using Admin.Desktop.View.Identity.Roles;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Controls;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Identity;

namespace Admin.Desktop.ViewModel.Identity.Roles
{
    public partial class RoleAddVM : ObservableObject, ITransientDependency
    {
        private readonly IIdentityRoleAppService _identityRoleAppService;
        private readonly ILogger<RoleAddVM> _logger;

        [ObservableProperty]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        public partial bool IsDefault { get; set; }

        [ObservableProperty]
        public partial bool IsPublic { get; set; }

        [ObservableProperty]
        public partial string DialogContainerToken { get; set; } = Guid.NewGuid().ToString();

        public RoleAddView Owner { get; private set; } = null!;

        public RoleAddVM(IIdentityRoleAppService identityRoleAppService, ILogger<RoleAddVM> logger)
        {
            _identityRoleAppService = identityRoleAppService;
            _logger = logger;
        }

        internal void Initial(RoleAddView owner)
        {
            Owner = owner;
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            var loadDialog = Dialog.Show<LoadingCircle>(DialogContainerToken);
            try
            {
                var result = await _identityRoleAppService.CreateAsync(new IdentityRoleCreateDto
                {
                    Name = Name,
                    IsDefault = IsDefault,
                    IsPublic = IsPublic,
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
