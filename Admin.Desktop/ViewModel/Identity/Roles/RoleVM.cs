using Admin.Desktop.View.Roles;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Controls;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Validation;
using MessageBox = HandyControl.Controls.MessageBox;

namespace Admin.Desktop.ViewModel.Roles
{
    public partial class RoleVM : ObservableObject, ITransientDependency
    {
        private readonly IIdentityRoleAppService _identityRoleAppService;
        private readonly ILogger<RoleVM> _logger;

        [ObservableProperty]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        public partial int PageIndex { get; set; } = 1;

        [ObservableProperty]
        public partial long TotalCount { get; set; }

        [ObservableProperty]
        public partial int PageSize { get; set; } = 30;

        [ObservableProperty]
        public partial ObservableCollection<IdentityRoleDto> Roles { get; set; } = new ObservableCollection<IdentityRoleDto>();

        [ObservableProperty]
        public partial string DialogContainerToken { get; set; } = Guid.NewGuid().ToString();

        [ObservableProperty]
        public partial Dictionary<string, Visibility> ButtonVis { get; set; } = new Dictionary<string, Visibility>();

        public RoleView Owner { get; private set; } = null!;

        public RoleVM(IIdentityRoleAppService identityRoleAppService, ILogger<RoleVM> logger)
        {
            _identityRoleAppService = identityRoleAppService;
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
                var result = await _identityRoleAppService.GetListAsync(new GetIdentityRolesInput
                {
                    Filter = Name,
                    SkipCount = (PageIndex - 1) * PageSize,
                    MaxResultCount = PageSize
                });
                TotalCount = result.TotalCount;
                Roles = new ObservableCollection<IdentityRoleDto>(result.Items);
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
        private async Task EditAsync(IdentityRoleDto role)
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
        private void EditPerm(IdentityRoleDto role)
        {
            var view = new EditRolePermissionView(role.Id);
            var result = view.ShowDialog();
            if (result == true)
            {
                MessageBox.Success("更新权限成功");
            }
        }


        [RelayCommand]
        private async Task Delete(IdentityRoleDto role)
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
                await _identityRoleAppService.DeleteAsync(role.Id);
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
                var roleIds = sender.Cast<IdentityRoleDto>().Select(x => x.Id).ToList();
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
                foreach (var roleId in roleIds)
                {
                    await _identityRoleAppService.DeleteAsync(roleId);
                }
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
