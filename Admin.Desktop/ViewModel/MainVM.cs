using Admin.Commons;
using Admin.Desktop.Resources.Langs;
using Admin.Desktop.Tools;
using Admin.Desktop.View;
using Admin.Desktop.View.Accounts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Tools;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Microsoft.Extensions.Logging;
using Microsoft.Web.WebView2.Wpf;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;
using Volo.Abp.DependencyInjection;
using MessageBox = HandyControl.Controls.MessageBox;
using TabItem = HandyControl.Controls.TabItem;

namespace Admin.Desktop.ViewModel
{
    public partial class MainVM : ObservableRecipient, ITransientDependency
    {
        private readonly string TitalPrefix = "Admin";
        private readonly ILogger<MainVM> _logger;

        [ObservableProperty]
        public partial string Title { get; set; } = string.Empty;

        [ObservableProperty]
        public partial int TabSelectedIndex { get; set; } = 0;

        [ObservableProperty]
        public partial string Version { get; set; } = null!;

        [ObservableProperty]
        public partial string UserName { get; set; } = string.Empty;

        [ObservableProperty]
        public partial ObservableCollection<NavDto> NavItems { get; set; } = new ObservableCollection<NavDto>();

        [ObservableProperty]
        public partial ObservableCollection<TabItem> TabItems { get; set; } = new ObservableCollection<TabItem>();
        [ObservableProperty]
        public partial string DialogContainerToken { get; set; } = Guid.NewGuid().ToString();
        public MainWindow Owner { get; private set; } = null!;

        public MainVM(ILogger<MainVM> logger)
        {
            _logger = logger;
        }

        public async Task InitialAsync(MainWindow owner)
        {
            var loadDialog = Dialog.Show(new LoadingCircle(), DialogContainerToken);
            try
            {
                Owner = owner;
                UserName = App.CurrentUser.UserName;
                Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? string.Empty;
                await Task.Run(() =>
                {
                    NavItems = BuiderNavItems();
                    var home = NavItems.FirstOrDefault(x => x.Type != NavType.Group);
                    if (home != null)
                    {
                        SetCurrentTabItem(home);
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                MessageBox.Error(ex.Message);
            }
            finally
            {
                loadDialog.Close();
            }
        }

        [RelayCommand]
        private void SwitchItem(NavDto navItem)
        {
            if (navItem.Content == null)
            {
                //菜单为空，不做处理
                return;
            }
            SetCurrentTabItem(navItem);
        }

        [RelayCommand]
        private void CurrentViewShow()
        {
            Owner.Show();
        }

        [RelayCommand]
        private void SwitchTheme(SkinType skinType)
        {
            try
            {
                var app = (App)Application.Current;

                var temps = app.Resources.MergedDictionaries.ToList();
                app.Resources.MergedDictionaries.Clear();

                app.Resources.MergedDictionaries.Add(ResourceHelper.GetSkin(skinType));
                app.Resources.MergedDictionaries.Add(new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml")
                });

                foreach (var item in temps)
                {
                    if (!item.Source.LocalPath.Contains("HandyControl"))
                    {
                        app.Resources.MergedDictionaries.Add(item);
                    }
                }
                // 夜间深色
                if (skinType == SkinType.Dark)
                {
                    LiveCharts.DefaultSettings.AddDarkTheme();
                }
                else
                {
                    // 白天浅色
                    LiveCharts.DefaultSettings.AddLightTheme();
                }
                Application.Current.MainWindow?.OnApplyTemplate();
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                MessageBox.Error(ex.Message);
            }
        }

        [RelayCommand]
        private void SwitchLang(string langName)
        {
            try
            {
                if (langName == LangProvider.Culture.Name)
                {
                    return;
                }

                ConfigHelper.Instance.SetLang(langName);
                LangProvider.Culture = new CultureInfo(langName);

                //var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                //config.AppSettings.Settings["Language"].Value = langName;
                //config.Save(ConfigurationSaveMode.Modified);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                MessageBox.Warning(ex.InnerException?.Message ?? ex.Message, "切换语言失败");
            }
        }

        [RelayCommand]
        private void MyProfile()
        {
            var nav = new NavDto
            {
                Id = Guid.Parse("F12CD9F3-2F14-4D13-965B-EFA60F207C3B"),
                Icon = "\xf013;",
                Name = "我的账户",
                Type = NavType.UserControl,
                Content = typeof(MyProfileView).FullName,
            };
            SwitchItem(nav);
        }

        [RelayCommand]
        private void Logout()
        {
            var view = new Login();
            view.Show();

            //NotifyIcon.ShowBalloonTip("HandyControl", "内容", NotifyIconInfoType.Info, "");
            Owner.NotifyIconContextContent.Visibility = Visibility.Collapsed;
            Owner.Close();
            Owner = null!;
        }

        private ObservableCollection<NavDto> BuiderNavItems()
        {
            var allNavItems = NavProvider.GetNavConfigs();
            //根据用户权限过滤菜单，待完善
            //var permissionResult = permissionApplicationService.GetPermissionDefinitions().GetAwaiter().GetResult();
            //var permissionNames = new List<string>();
            //var navItems = FilterPermissionTree(allNavItems, node => string.IsNullOrEmpty(node.PermissionName) || permissionNames.Contains(node.PermissionName));
            var navItems = FilterPermissionTree(allNavItems, node => true);
            return new ObservableCollection<NavDto>(navItems);
        }

        public static List<NavDto> FilterPermissionTree(IReadOnlyList<NavDto> nodes, Func<NavDto, bool> condition)
        {
            if (nodes == null)
            {
                return new List<NavDto>();
            }

            var result = new List<NavDto>();

            foreach (var node in nodes)
            {
                // 递归过滤子节点
                node.Items = FilterPermissionTree(node.Items, condition);

                // 满足条件 或 有子节点 → 保留
                if (condition(node) || node.Items.Any())
                {
                    result.Add(node);
                }
            }
            return result;
        }

        private void SetCurrentTabItem(NavDto navItem, object[]? parameters = null)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (navItem == null)
                    {
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(navItem.Content))
                    {
                        return;
                    }

                    //如果已经打开了，就切换到对应的Tab
                    var tabItemIdx = TabItems.FindIndex(x => x.Tag.ToString() == navItem.Id.ToString());
                    if (tabItemIdx >= 0)
                    {
                        TabSelectedIndex = tabItemIdx;
                        Title = $"{TitalPrefix} - {navItem.Name}";
                        return;
                    }

                    var isHome = navItem.Content == typeof(ConsoleView).FullName;
                    var tabHeaderText = new TextBlock();
                    if (!string.IsNullOrEmpty(navItem.Icon))
                    {
                        var iconRun = new Run
                        {
                            Text = navItem.Icon,
                            Foreground = (Brush)Application.Current.TryFindResource("DarkInfoBrush"),
                            FontFamily = (FontFamily)Application.Current.FindResource("FA_Regular"),
                        };
                        tabHeaderText.Inlines.Add(iconRun);
                        tabHeaderText.Inlines.Add(new Run
                        {
                            Text = "\u00A0",
                        });
                    }

                    tabHeaderText.Inlines.Add(new Run
                    {
                        Text = navItem.Name,
                    });
                    var tabItem = new TabItem
                    {
                        Header = tabHeaderText,
                        Tag = navItem.Id,
                        ShowCloseButton = !isHome,
                        ShowContextMenu = !isHome,
                    };

                    tabItem.Closing += (sender, args) =>
                    {
                        if (sender is TabItem && args is CancelRoutedEventArgs cancelArgs)
                        {
                            if (isHome)
                            {
                                cancelArgs.Cancel = true;
                                return;
                            }
                        }
                    };

                    if (navItem.Type == NavType.UserControl || navItem.Type == NavType.Page)
                    {
                        var contentType = Type.GetType(navItem.Content);
                        var constructor = contentType?.GetConstructor(Type.EmptyTypes);
                        if (typeof(UserControl).IsAssignableFrom(contentType))
                        {
                            tabItem.Content = (UserControl)constructor?.Invoke(parameters)!;
                        }
                        else if (typeof(Page).IsAssignableFrom(contentType))
                        {
                            tabItem.Content = new Frame
                            {
                                Content = (Page)constructor?.Invoke(parameters)!,
                                NavigationUIVisibility = NavigationUIVisibility.Hidden,
                            };
                        }
                        else
                        {
                            Growl.Error($"无法打开页面，类型错误：{navItem.Content}");
                            return;
                        }
                        Title = $"{TitalPrefix} - {navItem.Name}";
                        TabItems.Add(tabItem);
                        TabSelectedIndex = TabItems.IndexOf(tabItem);
                        return;
                    }

                    if (navItem.Type == NavType.Url)
                    {
                        tabItem.Content = new WebView2
                        {
                            Source = new Uri(navItem.Content),
                        };
                        Title = $"{TitalPrefix} - {navItem.Name}";
                        TabItems.Add(tabItem);
                        TabSelectedIndex = TabItems.IndexOf(tabItem);
                        return;
                    }
                }, DispatcherPriority.Background);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                Growl.Error($"打开页面失败：{ex.Message}");
            }
        }
    }
}
