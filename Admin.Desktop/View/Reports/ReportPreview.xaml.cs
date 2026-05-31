using Admin.Desktop.ViewModel.Reports;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Window = HandyControl.Controls.Window;

namespace Admin.Desktop.View.Reports
{
    /// <summary>
    /// ReportPreview.xaml 的交互逻辑
    /// </summary>
    public partial class ReportPreview : Window
    {
        private readonly ReportPreviewVM vm;

        public ReportPreview(string reportName)
        {
            InitializeComponent();
            vm = App.Current.Services.GetService<ReportPreviewVM>()!;
            if (string.IsNullOrWhiteSpace(reportName))
            {
                reportName = "About Microsoft Chart.frx";
            }
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Reports", reportName);
            vm.InitialVM(this, filePath);
            DataContext = vm;
        }
    }
}
