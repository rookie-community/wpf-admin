using HandyControl.Data;
using System.Windows;
using System.Windows.Controls;

namespace Admin.Desktop.UserControls
{
    /// <summary>
    /// PaginationPlus.xaml 的交互逻辑
    /// </summary>
    public partial class PaginationPlus : UserControl
    {
        public int TotalCount
        {
            get
            {
                var val = (int)GetValue(TotalCountProperty);
                if (val < 0) 
                {
                    return 0;
                }
                return val;
            }
            set => SetValue(TotalCountProperty, value);
        }
        public static readonly DependencyProperty TotalCountProperty =
            DependencyProperty.Register(nameof(TotalCount), typeof(int), typeof(PaginationPlus), new PropertyMetadata(0, Refresh));

        public int PageIndex
        {
            get
            {
                var val = (int)GetValue(PageIndexProperty);
                if (val > 0)
                {
                    return val;
                }
                return 1;
            }
            set => SetValue(PageIndexProperty, value);
        }
        public static readonly DependencyProperty PageIndexProperty =
            DependencyProperty.Register(nameof(PageIndex), typeof(int), typeof(PaginationPlus), new PropertyMetadata(1, Refresh));

        public int PageSize
        {
            get
            {
                var val = (int)GetValue(PageSizeProperty);
                if (val > 0)
                {
                    return val;
                }
                return 30;
            }
            set => SetValue(PageSizeProperty, value);
        }
        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register(nameof(PageSize), typeof(int), typeof(PaginationPlus), new PropertyMetadata(30, Refresh));

        public List<int> PageSizeOptions
        {
            get => (List<int>)GetValue(PageSizeOptionsProperty);
            set => SetValue(PageSizeOptionsProperty, value);
        }

        public static readonly DependencyProperty PageSizeOptionsProperty =
            DependencyProperty.Register(nameof(PageSizeOptions), typeof(List<int>), typeof(PaginationPlus),
                new PropertyMetadata(Enumerable.Range(1, 9).Select(x => x * 10).ToList()));

        // 事件
        public event Action<int, int> PageChanged;

        private void OnPageChanged() => PageChanged?.Invoke(PageIndex, PageSize);

        public PaginationPlus()
        {
            InitializeComponent();
            // 绑定内部事件
            PaginationMain.PageUpdated += (s, e) =>
            {
                PageIndex = e.Info;
                OnPageChanged();
            };
        }

        //private static bool _isInternalUpdating = false;

        // 刷新分页（只做一件事：计算页数）
        private static void Refresh(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = (PaginationPlus)d;
            c.PaginationMain.MaxPageCount = (int)Math.Ceiling(c.TotalCount / (double)c.PageSize);
            c.PaginationMain.PageIndex = c.PageIndex;
        }

        // 页码切换
        private void PaginationMain_PageUpdated(object sender, FunctionEventArgs<int> e)
        {
            PageIndex = e.Info;
            PageChanged?.Invoke(PageIndex, PageSize);
        }

        // 每页数量切换（最简单！）
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (((ComboBox)sender).SelectedItem is ComboBoxItem item)
            //{
            //    PageSize = int.Parse(item.Content.ToString()!);
            //    if (TotalCount == 0)
            //    {
            //    }
            //}
            PageIndex = 1; // 自动跳第一页
        }
    }
}
