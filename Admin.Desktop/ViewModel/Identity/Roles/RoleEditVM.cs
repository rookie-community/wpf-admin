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
    public partial class RoleEditVM : ObservableObject, ITransientDependency
    {
        private readonly IIdentityRoleAppService _identityRoleAppService;
        private readonly ILogger<RoleAddVM> _logger;
        private Guid _roleId;

        [ObservableProperty]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        public partial bool IsDefault { get; set; }

        [ObservableProperty]
        public partial bool IsPublic { get; set; }

        [ObservableProperty]
        public partial string DialogContainerToken { get; set; } = Guid.NewGuid().ToString();

        public RoleEditView Owner { get; private set; } = null!;

        public RoleEditVM(IIdentityRoleAppService identityRoleAppService, ILogger<RoleAddVM> logger)
        {
            _identityRoleAppService = identityRoleAppService;
            _logger = logger;
        }

        internal async Task InitialAsync(RoleEditView owner, Guid roleId)
        {
            var loadDialog = Dialog.Show(new LoadingCircle(), DialogContainerToken);
            try
            {
                Owner = owner;
                _roleId = roleId;
                var result = await _identityRoleAppService.GetAsync(roleId);
                Name = result.Name;
                IsDefault = result.IsDefault;
                IsPublic = result.IsPublic;
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
                var result = await _identityRoleAppService.UpdateAsync(_roleId, new IdentityRoleUpdateDto
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
