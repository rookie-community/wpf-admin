using Admin.Desktop.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace Admin.Desktop.View
{
    /// <summary>
    /// Console.xaml 的交互逻辑
    /// </summary>
    public partial class ConsoleView : UserControl
    {
        private ConsoleVM vm;
        public ConsoleView()
        {
            InitializeComponent();
            vm = App.Current.Services.GetService<ConsoleVM>()!;
            vm.Initial(this);
            DataContext = vm;
        }
    }
}
