using Admin.Desktop.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Windows.Controls;
using Window = HandyControl.Controls.Window;

namespace Admin.Desktop.View
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
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
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Hide();
            e.Cancel = true;
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
