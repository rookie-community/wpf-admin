using Admin.Desktop.View.Reports;
using CommunityToolkit.Mvvm.ComponentModel;
using FastReport;
using Volo.Abp.DependencyInjection;

namespace Admin.Desktop.ViewModel.Reports
{
    public partial class ReportPreviewVM : ObservableObject, ITransientDependency
    {
        [ObservableProperty]
        private Report report = new Report();
        public ReportPreview Owner { get; private set; } = null!;

        public ReportPreviewVM()
        {
        }

        public void InitialVM(ReportPreview owner, string reportName)
        {
            Owner = owner;
            Report.Load(reportName);
            Report.PrepareAsync(Owner.previewControl);
        }
    }
}
