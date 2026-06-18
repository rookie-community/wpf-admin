using Admin.Desktop.View.Identity.OrganizationUnits;
using Admin.OrganizationUnits;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Controls;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client;

namespace Admin.Desktop.ViewModel.Identity.OrganizationUnits
{
    public partial class OrganizationUnitAddVM : ObservableObject, ITransientDependency
    {
        private readonly IOrganizationUnitAppService _organizationUnitAppService;
        private readonly ILogger<OrganizationUnitAddVM> _logger;

        [ObservableProperty]
        public partial string DisplayName { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string DialogContainerToken { get; set; } = Guid.NewGuid().ToString();

        public OrganizationUnitAddView Owner { get; private set; } = null!;

        public OrganizationUnitAddVM(IOrganizationUnitAppService organizationUnitAppService, ILogger<OrganizationUnitAddVM> logger)
        {
            _organizationUnitAppService = organizationUnitAppService;
            _logger = logger;
        }

        internal void Initial(OrganizationUnitAddView owner)
        {
            Owner = owner;
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(DisplayName))
            {
                MessageBox.Warning("请输入机构名称");
                return;
            }

            var loadDialog = Dialog.Show<LoadingCircle>(DialogContainerToken);
            try
            {
                var result = await _organizationUnitAppService.CreateAsync(new CreateOrganizationUnitInput
                {
                    ParentId = null, // 根机构，ParentId 为 null
                    DisplayName = DisplayName,
                    SortOrder = 0,
                });
                Owner.DialogResult = true;
                Owner.Close();
            }
            catch (AbpRemoteCallException abpRemoteCallException)
            {
                _logger.LogException(abpRemoteCallException);
                MessageBox.Error(abpRemoteCallException.Details, abpRemoteCallException.Message);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                MessageBox.Error(ex.Message);
            }
            finally
            {
                loadDialog.Close();
            }
        }

        [RelayCommand]
        private void Cancel()
        {
            Owner.Close();
        }
    }
}
