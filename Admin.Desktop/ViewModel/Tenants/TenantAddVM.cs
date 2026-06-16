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
    public partial class TenantAddVM : ObservableObject, ITransientDependency
    {
        private readonly ITenantAppService _tenantAppService;
        private readonly ILogger<TenantAddVM> _logger;

        [ObservableProperty]
        public partial string Name { get; set; } = null!;

        [ObservableProperty]
        public partial string Email { get; set; } = null!;

        [ObservableProperty]
        public partial string Password { get; set; } = null!;

        [ObservableProperty]
        public partial string DialogContainerToken { get; set; } = Guid.NewGuid().ToString();
        public TenantAddView Owner { get; private set; } = null!;

        public TenantAddVM(ITenantAppService tenantAppService, ILogger<TenantAddVM> logger)
        {
            _tenantAppService = tenantAppService;
            _logger = logger;
        }

        internal void Initial(TenantAddView owner)
        {
            Owner = owner;
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            var loadDialog = Dialog.Show<LoadingCircle>(DialogContainerToken);
            try
            {
                var result = await _tenantAppService.CreateAsync(new TenantCreateDto
                {
                    Name = Name,
                    AdminEmailAddress = Email,
                    AdminPassword = Password,
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
