using Admin.Desktop.View.Reports;
using CommunityToolkit.Mvvm.ComponentModel;
using FastReport;
using System.IO;
using Volo.Abp.DependencyInjection;

namespace Admin.Desktop.ViewModel.Reports
{
    public partial class ReportDesignVM : ObservableObject, ITransientDependency
    {
        [ObservableProperty]
        private Report report = new Report();

        public ReportDesign Owner { get; private set; } = null!;

        public ReportDesignVM()
        {
        }

        public void InitialVM(ReportDesign owner, string? fileName = null)
        {
            Owner = owner;
            if (fileName == null)
            {
                //加载报表
                var reportName = "About Microsoft Chart.frx";
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Reports", reportName);
                Report.Load(filePath);
            }

            //创建数据源
            //var list = CreateBusinessObject();
            //绑定数据源
            //report.RegisterData(list, "Categories");
            Owner.designerControl.Report = Report;
        }

        public void InitialVM(ReportDesign owner, Stream stream)
        {
            Owner = owner;
            Report.Load(stream);
            Owner.designerControl.Report = Report;
        }
    }
}
