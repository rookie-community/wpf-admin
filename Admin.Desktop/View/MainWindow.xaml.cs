using Admin.Desktop.Resources.Langs;
using Admin.Desktop.ViewModel;
using HandyControl.Data;
using HandyControl.Tools;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using Window = HandyControl.Controls.Window;
using MessageBox = HandyControl.Controls.MessageBox;

namespace Admin.Desktop.View
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private string currentLang;
        private readonly MainVM vm;

        public MainWindow()
        {
            InitializeComponent();
            vm = App.Current.Services.GetService<MainVM>()!;
            vm.Initial(this);
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
                ;
                Application.Current.MainWindow?.OnApplyTemplate();
            }
        }
    }
}
