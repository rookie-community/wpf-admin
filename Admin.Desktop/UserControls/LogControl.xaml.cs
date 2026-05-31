using Admin.Commons;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Admin.Desktop.UserControls
{
    /// <summary>
    /// LogControl.xaml 的交互逻辑
    /// </summary>
    public partial class LogControl : UserControl
    {
        public LogControl()
        {
            InitializeComponent();
        }

        public ObservableCollection<LogInfo> ItemsSource
        {
            get { return (ObservableCollection<LogInfo>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LogInfos.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(ObservableCollection<LogInfo>), typeof(LogControl), new PropertyMetadata(default(ObservableCollection<LogInfo>)));

        private void ListBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.C && Keyboard.Modifiers == ModifierKeys.Control)
            {
                var sb = new StringBuilder();
                foreach (var item in lst_log.SelectedItems)
                {
                    if (item is LogInfo log)
                    {
                        sb.AppendLine(log.LogMessages);
                    }
                }
                Clipboard.SetText(sb.ToString());
                e.Handled = true; // 防止其他控件处理此事件
            }
        }
    }

    public sealed class LogInfo : LogDto
    {
        public SolidColorBrush BackgroundBrush => Level switch
        {
            LogLevel.Information => (SolidColorBrush)Application.Current.TryFindResource("DarkSuccessBrush"),
            LogLevel.Warning => (SolidColorBrush)Application.Current.TryFindResource("DarkWarningBrush"),
            LogLevel.Error => (SolidColorBrush)Application.Current.TryFindResource("DarkDangerBrush"),
            LogLevel.Critical => (SolidColorBrush)Application.Current.TryFindResource("DarkDangerBrush"),
            _ => (SolidColorBrush)Application.Current.TryFindResource("DarkPrimaryBrush"),
        };

        public Style BorderTipStyle => Level switch
        {
            LogLevel.Information => (Style)Application.Current.TryFindResource("BorderTipSuccess"),
            LogLevel.Warning => (Style)Application.Current.TryFindResource("BorderTipWarning"),
            LogLevel.Error => (Style)Application.Current.TryFindResource("BorderTipDanger"),
            LogLevel.Critical => (Style)Application.Current.TryFindResource("BorderTipDanger"),
            _ => (Style)Application.Current.TryFindResource("BorderTipInfo"),
        };

        public Style TextBlockStyle => Level switch
        {
            LogLevel.Information => (Style)Application.Current.TryFindResource("TextBlockDefaultInfo"),
            LogLevel.Warning => (Style)Application.Current.TryFindResource("TextBlockDefaultWarning"),
            LogLevel.Error => (Style)Application.Current.TryFindResource("TextBlockDefaultAccent"),
            LogLevel.Critical => (Style)Application.Current.TryFindResource("TextBlockDefaultDanger"),
            _ => (Style)Application.Current.TryFindResource("TextBlockDefault")
        };

        public string LogMessages => $"[{CreateTime}]:{Messages}";
    }
}
