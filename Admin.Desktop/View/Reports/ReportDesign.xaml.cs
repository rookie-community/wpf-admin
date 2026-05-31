using Admin.Desktop.ViewModel.Reports;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace Admin.Desktop.View.Reports
{
    /// <summary>
    /// ReportDesign.xaml 的交互逻辑
    /// </summary>
    public partial class ReportDesign : UserControl
    {
        private readonly ReportDesignVM vm;
        public ReportDesign()
        {
            InitializeComponent();
            vm = App.Current.Services.GetService<ReportDesignVM>()!;
            vm.InitialVM(this);
            DataContext = vm;
        }
    }
}
