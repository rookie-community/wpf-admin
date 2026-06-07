using Admin.Commons;
using Admin.Desktop.Tools;
using Admin.Desktop.View.Roles;
using Admin.Permissions;
using Admin.Roles;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Controls;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using Volo.Abp.DependencyInjection;

namespace Admin.Desktop.ViewModel.Roles
{
    public partial class EditRolePermissionVM : ObservableObject, ITransientDependency
    {
        [ObservableProperty]
        private ObservableCollection<NavDto> permissions = new ObservableCollection<NavDto>();

        [ObservableProperty]
        private string dialogContainerToken = Guid.NewGuid().ToString();

        public EditRolePermissionView Owner { get; private set; } = null!;
        private readonly IRoleApplicationService _roleApplicationService;
        private readonly IPermissionApplicationService _permissionApplicationService;
        private readonly ILogger<EditRolePermissionVM> _logger;

        public EditRolePermissionVM(IRoleApplicationService roleApplicationService, IPermissionApplicationService permissionApplicationService, ILogger<EditRolePermissionVM> logger)
        {
            _roleApplicationService = roleApplicationService;
            _permissionApplicationService = permissionApplicationService;
            _logger = logger;
        }

        internal async Task InitialAsync(EditRolePermissionView owner, Guid roleId)
        {
            var loadDialog = Dialog.Show(new LoadingCircle(), DialogContainerToken);
            try
            {
                Owner = owner;
                var permissionResult = await _permissionApplicationService.GetPermissionDefinitions();
                //Permissions = new ObservableCollection<PermissionDefinitionDto>(permissionResult);
                Permissions = new ObservableCollection<NavDto>(NavProvider.GetNavConfigs());
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
        public void Save()
        {

        }

        [RelayCommand]
        public void Cancel()
        {
            Owner.Close();
        }
    }
}
