using HandyControl.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Admin.Desktop.UserControls
{
    public partial class PaginationPlus : UserControl
    {
        #region 依赖属性

        public long TotalCount
        {
            get => (long)GetValue(TotalCountProperty);
            set => SetValue(TotalCountProperty, value);
        }
        public static readonly DependencyProperty TotalCountProperty =
            DependencyProperty.Register(nameof(TotalCount), typeof(long), typeof(PaginationPlus), new PropertyMetadata(0L, Refresh));

        public int PageIndex
        {
            get => (int)GetValue(PageIndexProperty);
            set => SetValue(PageIndexProperty, value);
        }
        public static readonly DependencyProperty PageIndexProperty =
            DependencyProperty.Register(nameof(PageIndex), typeof(int), typeof(PaginationPlus), new PropertyMetadata(1, Refresh));

        public int PageSize
        {
            get => (int)GetValue(PageSizeProperty);
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
            DependencyProperty.Register(nameof(PageSizeOptions), typeof(List<int>), typeof(PaginationPlus), new PropertyMetadata(null));

        public ICommand PageChangedCommand
        {
            get => (ICommand)GetValue(PageChangedCommandProperty);
            set => SetValue(PageChangedCommandProperty, value);
        }
        public static readonly DependencyProperty PageChangedCommandProperty =
            DependencyProperty.Register(nameof(PageChangedCommand), typeof(ICommand), typeof(PaginationPlus));

        #endregion

        public event Action<int, int>? PageChanged;

        public PaginationPlus()
        {
            InitializeComponent();

            if (PageSizeOptions == null)
            {
                PageSizeOptions = Enumerable.Range(1, 9).Select(x => x * 10).ToList();
            }

            PaginationMain.PageUpdated += OnPageUpdated;

            this.Unloaded += (s, e) =>
            {
                PaginationMain.PageUpdated -= OnPageUpdated;
            };
        }

        /// <summary>
        /// 内部 UI 页码切换事件处理 (用户点击 UI 时触发)
        /// </summary>
        private void OnPageUpdated(object? sender, FunctionEventArgs<int> e)
        {
            if (PageIndex != e.Info)
            {
                PageIndex = e.Info;
            }
        }

        /// <summary>
        /// 核心：触发命令 + 事件 (传参方案)
        /// </summary>
        private void NotifyPageChanged()
        {
            int currentIndex = PageIndex;
            int currentSize = PageSize;

            PageChanged?.Invoke(currentIndex, currentSize);

            if (PageChangedCommand != null)
            {
                var args = new Tuple<int, int>(currentIndex, currentSize);

                if (PageChangedCommand.CanExecute(args))
                {
                    PageChangedCommand.Execute(args);
                }
            }
        }

        /// <summary>
        /// 依赖属性变更回调：刷新总页数、同步 UI 状态 & 统一触发数据更新
        /// </summary>
        private static void Refresh(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = (PaginationPlus)d;
            if (c.PaginationMain == null || c.PageSize <= 0 || c.TotalCount < 0)
            {
                return;
            }

            bool isPageSizeChanged = e.Property == PageSizeProperty;
            bool isPageIndexChanged = e.Property == PageIndexProperty;

            // 1. 更新 UI 上的最大页数
            int newMaxPage = (int)Math.Ceiling(c.TotalCount / (double)c.PageSize);
            if (c.PaginationMain.MaxPageCount != newMaxPage)
            {
                c.PaginationMain.PageUpdated -= c.OnPageUpdated;
                c.PaginationMain.MaxPageCount = newMaxPage;
                c.PaginationMain.PageUpdated += c.OnPageUpdated;
            }

            // 2. 处理 PageSize 改变时的 PageIndex 重置
            if (isPageSizeChanged)
            {
                // 交互规范：切换每页条数时，必须回到第 1 页
                if (c.PageIndex != 1)
                {
                    // 将 PageIndex 设为 1，这会再次触发 Refresh (e.Property == PageIndexProperty)
                    // 在此处直接 return，等待 PageIndex 变更的回调来统一触发数据加载，避免带着旧页码请求
                    c.PageIndex = 1;
                    return;
                }
            }

            // 3. 保持内外页码同步：将外层的 PageIndex 同步给内部 HandyControl UI 控件
            if (isPageIndexChanged || isPageSizeChanged)
            {
                if (c.PaginationMain.PageIndex != c.PageIndex)
                {
                    c.PaginationMain.PageUpdated -= c.OnPageUpdated;
                    c.PaginationMain.PageIndex = c.PageIndex;
                    c.PaginationMain.PageUpdated += c.OnPageUpdated;
                }
            }

            // 4. 当分页核心参数变化时，通知外部刷新数据
            if ((isPageIndexChanged || isPageSizeChanged) && c.IsLoaded)
            {
                c.NotifyPageChanged();
            }
        }
    }
}