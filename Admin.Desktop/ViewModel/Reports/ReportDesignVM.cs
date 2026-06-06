using Admin.Desktop.View.Reports;
using CommunityToolkit.Mvvm.ComponentModel;
using FastReport;
using HandyControl.Controls;
using Microsoft.Extensions.Logging;
using System.IO;
using Volo.Abp.DependencyInjection;

namespace Admin.Desktop.ViewModel.Reports
{
    public partial class ReportDesignVM : ObservableObject, ITransientDependency
    {
        [ObservableProperty]
        private Report report = new Report();

        [ObservableProperty]
        private string dialogContainerToken = Guid.NewGuid().ToString();

        private readonly ILogger<ReportDesignVM> _logger;

        public ReportDesign Owner { get; private set; } = null!;

        public ReportDesignVM(ILogger<ReportDesignVM> logger)
        {
            _logger = logger;
        }

        public async Task InitialAsync(ReportDesign owner, string? fileName = null)
        {
            var loadDialog = Dialog.Show(new LoadingCircle(), DialogContainerToken);
            try
            {
                Owner = owner;

                await Task.Run(() =>
                {
                    if (fileName == null)
                    {
                        //加载报表
                        var reportName = "About Microsoft Chart.frx";
                        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Reports", reportName);
                        Report.Load(filePath);
                        //创建数据源
                        //var list = CreateBusinessObject();
                        //绑定数据源
                        //report.RegisterData(list, "Categories");
                    }
                });
                Owner.designerControl.Report = Report;
            }
            catch (Exception ex)
            {
                MessageBox.Error(ex.Message);
            }
            finally
            {
                loadDialog.Close();
            }
        }

        public void InitialVM(ReportDesign owner, Stream stream)
        {
            Owner = owner;
            Report.Load(stream);
            Owner.designerControl.Report = Report;
        }
    }
}
