using Admin.Desktop.View.Roles;
using Admin.Permissions;
using Admin.Roles;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Controls;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Validation;

namespace Admin.Desktop.ViewModel.Roles
{
    public partial class RoleVM : ObservableObject, ITransientDependency
    {
        [ObservableProperty]
        private string roleCode = string.Empty;

        [ObservableProperty]
        private string roleName = string.Empty;

        [ObservableProperty]
        private int pageIndex = 0;

        [ObservableProperty]
        private long totalCount;

        [ObservableProperty]
        private int pageSize = 30;

        [ObservableProperty]
        private string dialogContainerToken = Guid.NewGuid().ToString();
        private readonly IRoleApplicationService _roleApplicationService;
        private readonly IPermissionApplicationService _permissionApplicationService;
        private readonly ILogger<RoleVM> _logger;

        public RoleView Owner { get; private set; } = null!;

        public RoleVM(IRoleApplicationService roleApplicationService, IPermissionApplicationService permissionApplicationService, ILogger<RoleVM> logger)
        {
            _roleApplicationService = roleApplicationService;
            _permissionApplicationService = permissionApplicationService;
            _logger = logger;
        }

        internal void Initial(RoleView owner)
        {
            Owner = owner;
        }

        [RelayCommand]
        private async Task SearchAsync()
        {
            var loadDialog = Dialog.Show(new LoadingCircle(), DialogContainerToken);
            try
            {
                await Task.Delay(1000 * 5);
                //var result = await _userApplicationService.GetListAsync(new GetUserListDto
                //{
                //    UserName = UserName,
                //    PhoneNumber = PhoneNumber,
                //    Email = Email
                //});
                //Users = new ObservableCollection<UserDto>(result.Items);
            }
            catch (AbpValidationException abpEx)
            {
                var errorMessages = abpEx.ValidationErrors.Select(x => x.ErrorMessage);
                MessageBox.Error(string.Join('.', errorMessages), abpEx.Message);
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

        [RelayCommand]
        private void Reset()
        {
            RoleCode = string.Empty;
            RoleName = string.Empty;
        }
    }
}
