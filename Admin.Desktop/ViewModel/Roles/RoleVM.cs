using Admin.Desktop.View.Roles;
using Admin.Permissions;
using Admin.Roles;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Controls;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Validation;
using MessageBox = HandyControl.Controls.MessageBox;

namespace Admin.Desktop.ViewModel.Roles
{
    public partial class RoleVM : ObservableObject, ITransientDependency
    {
        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        private int pageIndex = 1;

        [ObservableProperty]
        private long totalCount;

        [ObservableProperty]
        private int pageSize = 30;

        [ObservableProperty]
        private ObservableCollection<RoleDto> roles = new ObservableCollection<RoleDto>();

        [ObservableProperty]
        private string dialogContainerToken = Guid.NewGuid().ToString();

        [ObservableProperty]
        private Dictionary<string, Visibility> buttonVis = new Dictionary<string, Visibility>();

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

        internal async Task InitialAsync(RoleView owner)
        {
            Owner = owner;
            await SearchCommand.ExecuteAsync(null);
        }

        [RelayCommand]
        private async Task SearchAsync()
        {
            var loadDialog = Dialog.Show(new LoadingCircle(), DialogContainerToken);
            try
            {
                var result = await _roleApplicationService.GetListAsync(new GetRoleListDto
                {
                    Name = Name,
                    SkipCount = (PageIndex - 1) * PageSize,
                    MaxResultCount = PageSize
                });
                TotalCount = result.TotalCount;
                Roles = new ObservableCollection<RoleDto>(result.Items);
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
            Name = string.Empty;
        }

        [RelayCommand]
        private async Task AddAsync()
        {
            try
            {
                await SearchCommand.ExecuteAsync(null);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                MessageBox.Error(ex.Message);
            }
        }

        [RelayCommand]
        private async Task EditAsync(RoleDto role)
        {
            try
            {
                await SearchCommand.ExecuteAsync(null);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                MessageBox.Error(ex.Message);
            }
        }

        [RelayCommand]
        private void EditPerm(RoleDto role)
        {
            var view = new EditRolePermissionView(role.Id);
            var result = view.ShowDialog();
            if (result == true)
            {
                MessageBox.Success("更新权限成功");
            }
        }


        [RelayCommand]
        private async Task Delete(RoleDto role)
        {
            Dialog? loadDialog = null;
            try
            {
                var result = MessageBox.Ask($"确认删除数据？");
                if (result != MessageBoxResult.OK)
                {
                    return;
                }
                loadDialog = Dialog.Show<LoadingCircle>();
                await _roleApplicationService.BatchDelete(new List<Guid> { role.Id });
                await SearchCommand.ExecuteAsync(null);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                MessageBox.Error(ex.Message);
            }
            finally
            {
                loadDialog?.Close();
            }
        }

        [RelayCommand]
        private async Task BatchDelete(IList sender)
        {
            Dialog? loadDialog = null;
            try
            {
                var roleIds = sender.Cast<RoleDto>().Select(x => x.Id).ToList();
                if (roleIds.Count == 0)
                {
                    MessageBox.Warning("未选中任何数据！");
                    return;
                }
                var result = MessageBox.Ask($"确认删除选中数据？");
                if (result != MessageBoxResult.OK)
                {
                    return;
                }
                loadDialog = Dialog.Show<LoadingCircle>();
                await _roleApplicationService.BatchDelete(roleIds);
                await SearchCommand.ExecuteAsync(null);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                MessageBox.Error(ex.Message);
            }
            finally
            {
                loadDialog?.Close();
            }
        }
    }
}
