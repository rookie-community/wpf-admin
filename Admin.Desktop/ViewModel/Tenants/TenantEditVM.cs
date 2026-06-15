using Admin.Desktop.View.Tenants;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Controls;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.TenantManagement;

namespace Admin.Desktop.ViewModel.Tenants
{
    public partial class TenantEditVM : ObservableObject, ITransientDependency
    {
        private readonly ITenantAppService _tenantAppService;
        private readonly ILogger<TenantEditVM> _logger;
        private Guid _tenantId;

        [ObservableProperty]
        public partial string Name { get; set; } = null!;

        [ObservableProperty]
        public partial string DialogContainerToken { get; set; } = Guid.NewGuid().ToString();
        public TenantEditView Owner { get; private set; } = null!;

        public TenantEditVM(ITenantAppService tenantAppService, ILogger<TenantEditVM> logger)
        {
            _tenantAppService = tenantAppService;
            _logger = logger;
        }

        internal async Task InitialAsync(TenantEditView owner, Guid tenantId)
        {
            var loadDialog = Dialog.Show(new LoadingCircle(), DialogContainerToken);
            try
            {
                Owner = owner;
                _tenantId = tenantId;
                var result = await _tenantAppService.GetAsync(tenantId);
                Name = result.Name;
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
        private async Task SaveAsync()
        {
            var loadDialog = Dialog.Show(new LoadingCircle(), DialogContainerToken);
            try
            {
                var result = await _tenantAppService.UpdateAsync(_tenantId, new TenantUpdateDto
                {
                    Name = Name,
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
