using Admin.Desktop.View.Roles;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Controls;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;

namespace Admin.Desktop.ViewModel.Roles
{
    public partial class EditRolePermissionVM : ObservableObject, ITransientDependency
    {
        private readonly ILogger<EditRolePermissionVM> _logger;

        //[ObservableProperty]
        //private ObservableCollection<NavDto> permissions = new ObservableCollection<NavDto>();

        [ObservableProperty]
        public partial string DialogContainerToken { get; set; } = Guid.NewGuid().ToString();

        public EditRolePermissionView Owner { get; private set; } = null!;

        public EditRolePermissionVM(ILogger<EditRolePermissionVM> logger)
        {
            _logger = logger;
        }

        internal async Task InitialAsync(EditRolePermissionView owner, Guid roleId)
        {
            var loadDialog = Dialog.Show(new LoadingCircle(), DialogContainerToken);
            try
            {
                Owner = owner;
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
