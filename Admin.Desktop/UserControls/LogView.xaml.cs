using HandyControl.Data;
using HandyControl.Tools;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Admin.Desktop.UserControls
{
    /// <summary>
    /// LogView.xaml 的交互逻辑
    /// </summary>
    public partial class LogView : UserControl
    {
        private readonly Lock _writeLocker = new Lock();
        public LogView()
        {
            InitializeComponent();
        }

        public int MaxLineCount
        {
            get => (int)GetValue(MaxLineCountProperty);
            set => SetValue(MaxLineCountProperty, value);
        }

        public static readonly DependencyProperty MaxLineCountProperty =
            DependencyProperty.Register(
                nameof(MaxLineCount),
                typeof(int),
                typeof(LogView),
                new PropertyMetadata(100)); // 默认100行

        /// <summary>
        /// 通用输出日志
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="dateTime"></param>
        /// <param name="color"></param>
        private void Write(string msg, DateTime? dateTime = null, SolidColorBrush? color = null)
        {
            lock (_writeLocker)
            {
                // 记录当前滚动位置
                var currentOffset = rtbLog.VerticalOffset;
                // 超出最大行删除最旧一条
                while (rtbLog.Document.Blocks.Count > MaxLineCount)
                {
                    rtbLog.Document.Blocks.Remove(rtbLog.Document.Blocks.FirstBlock);
                }

                var para = new Paragraph
                {
                    Margin = new Thickness(0, 1, 0, 1),
                    Padding = new Thickness(0)
                };

                // 时间
                para.Inlines.Add(new Run($"[{dateTime ?? DateTime.Now}] ")
                {
                    Foreground = GetHcBrush(ResourceToken.SecondaryTextBrush)
                    //Foreground = Foreground = Brushes.Gray
                });

                var contentRun = new Run(msg);
                if (color != null)
                {
                    contentRun.Foreground = color;
                }
                // 日志内容 
                para.Inlines.Add(contentRun);
                rtbLog.Document.Blocks.Add(para);
                rtbLog.ScrollToVerticalOffset(currentOffset);
            }
        }

        public void Info(string msg, DateTime? dateTime = null) => Write(msg, dateTime, null);

        public void Success(string msg, DateTime? dateTime = null) => Write(msg, dateTime, GetHcBrush(ResourceToken.DarkSuccessBrush));

        public void Warn(string msg, DateTime? dateTime = null) => Write(msg, dateTime, GetHcBrush(ResourceToken.DarkWarningBrush));

        public void Error(string msg, DateTime? dateTime = null) => Write(msg, dateTime, GetHcBrush(ResourceToken.DarkDangerBrush));

        /// <summary>
        /// 清空全部日志
        /// </summary>
        public void Clear()
        {
            rtbLog.Document.Blocks.Clear();
        }

        private static SolidColorBrush? GetHcBrush(string resourceToken)
        {
            var brush = ResourceHelper.GetResource<SolidColorBrush>(resourceToken);
            // 资源找不到时给默认黑色，防止null报错
            return brush ?? Brushes.Black;
        }
    }
}
