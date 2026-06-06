using Admin.Commons;
using Admin.Desktop.Tools;
using Admin.Desktop.View;
using Admin.Permissions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Controls;
using HandyControl.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Web.WebView2.Wpf;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;
using Volo.Abp.DependencyInjection;
using TabItem = HandyControl.Controls.TabItem;

namespace Admin.Desktop.ViewModel
{
    public partial class MainVM : ObservableRecipient, ITransientDependency
    {
        [ObservableProperty]
        private string title = string.Empty;

        [ObservableProperty]
        private int tabSelectedIndex = 0;

        [ObservableProperty]
        private ObservableCollection<NavDto> navItems = new ObservableCollection<NavDto>();

        [ObservableProperty]
        private ObservableCollection<TabItem> tabItems = new ObservableCollection<TabItem>();

        public MainWindow Owner { get; private set; } = null!;

        private readonly IPermissionApplicationService permissionApplicationService;

        private readonly string TitalPrefix = "Admin";
        private ILogger<MainVM> _logger;

        public MainVM(IPermissionApplicationService permissionApplicationService, ILogger<MainVM> logger)
        {
            this.permissionApplicationService = permissionApplicationService;
            _logger = logger;
        }

        public void Initial(MainWindow owner)
        {
            Owner = owner;
            NavItems = BuiderNavItems();
            InitialTabs();
        }

        [RelayCommand]
        private void SwitchItem(NavDto navItem)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (navItem.Content == null)
                {
                    //菜单为空，不做处理
                    return;
                }
                SetCurrentTabItem(navItem);
            }, DispatcherPriority.Background);
        }

        [RelayCommand]
        private void CurrentViewShow()
        {
            Owner.Show();
        }

        private void InitialTabs()
        {
            var home = NavItems.FirstOrDefault();
            if (home != null)
            {
                SetCurrentTabItem(home);
            }
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
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                Growl.Error($"打开页面失败：{ex.Message}");
            }
        }
    }
}
