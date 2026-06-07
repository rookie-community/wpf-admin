using Admin.Desktop.Resources.Langs;
using Admin.Desktop.ViewModel;
using HandyControl.Data;
using HandyControl.Tools;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using MessageBox = HandyControl.Controls.MessageBox;
using Window = HandyControl.Controls.Window;

namespace Admin.Desktop.View
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private string currentLang;
        private readonly MainVM vm;
        private bool _isLoaded;

        public MainWindow()
        {
            InitializeComponent();
            vm = App.Current.Services.GetService<MainVM>()!;
            Loaded += async (s, e) =>
            {
                if (_isLoaded)
                {
                    // 避免重复加载数据
                    return;
                }
                _isLoaded = true;
                await vm.InitialAsync(this);
            };
            DataContext = vm;
            currentLang = LangProvider.Culture.Name;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void ButtonLangs_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (e.OriginalSource is Button { Tag: string langName })
                {
                    PopupConfig.IsOpen = false;
                    if (langName == currentLang)
                    {
                        return;
                    }

                    ConfigHelper.Instance.SetLang(langName);
                    LangProvider.Culture = new CultureInfo(langName);

                    currentLang = langName;
                    //var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    //config.AppSettings.Settings["Language"].Value = langName;
                    //config.Save(ConfigurationSaveMode.Modified);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Warning(ex.InnerException?.Message ?? ex.Message, "切换语言失败");
            }
        }

        private void ButtonConfig_OnClick(object sender, RoutedEventArgs e) => PopupConfig.IsOpen = true;

        private void ButtonSkins_OnClick(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Button { Tag: SkinType skinType })
            {
                PopupConfig.IsOpen = false;

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
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1 && e.AddedItems[0] is TabItem tabItem)
            {
                //vm.SetCurrentTreeViewSelectdItemCommand.Execute(tabItem);
            }
        }
    }
}
