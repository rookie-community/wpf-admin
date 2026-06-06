using Admin.Desktop.View.Roles;
using Admin.Roles;
using CommunityToolkit.Mvvm.ComponentModel;
using HandyControl.Controls;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;

namespace Admin.Desktop.ViewModel.Roles
{
    public partial class EditRolePermissionVM : ObservableObject, ITransientDependency
    {

        [ObservableProperty]
        private string dialogContainerToken = Guid.NewGuid().ToString();

        public EditRolePermissionView Owner = null!;
        private readonly IRoleApplicationService _roleApplicationService;
        private readonly ILogger<EditRolePermissionVM> _logger;

        public EditRolePermissionVM(IRoleApplicationService roleApplicationService, ILogger<EditRolePermissionVM> logger)
        {
            _roleApplicationService = roleApplicationService;
            _logger = logger;
        }

        internal async Task InitialAsync(EditRolePermissionView owner)
        {
            var loadDialog = Dialog.Show(new LoadingCircle(), DialogContainerToken);
            try
            {
                Owner = owner;
                await Task.Delay(1000);
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
    }
}
