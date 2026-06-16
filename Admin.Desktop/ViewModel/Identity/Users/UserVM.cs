using Admin.Desktop.View.Identity.Users;
using Admin.Desktop.View.Permissions;
using Admin.Desktop.View.Users;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Controls;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Validation;
using MessageBox = HandyControl.Controls.MessageBox;

namespace Admin.Desktop.ViewModel.Users
{
    public partial class UserVM : ObservableObject, ITransientDependency
    {
        private readonly IIdentityUserAppService _identityUserAppService;
        private readonly IPermissionChecker _permissionChecker;
        private readonly ILogger<UserVM> _logger;

        [ObservableProperty]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        public partial ObservableCollection<IdentityUserDto> Users { get; set; } = new ObservableCollection<IdentityUserDto>();

        [ObservableProperty]
        public partial int PageIndex { get; set; } = 1;

        [ObservableProperty]
        public partial long TotalCount { get; set; }

        [ObservableProperty]
        public partial int PageSize { get; set; } = 30;

        [ObservableProperty]
        public partial string DialogContainerToken { get; set; } = Guid.NewGuid().ToString();

        [ObservableProperty]
        public partial Dictionary<string, Visibility> BtnPerms { get; set; } = new Dictionary<string, Visibility>();

        public UserView Owner { get; private set; } = null!;

        public UserVM(IIdentityUserAppService identityUserAppService, IPermissionChecker permissionChecker, ILogger<UserVM> logger)
        {
            _identityUserAppService = identityUserAppService;
            _permissionChecker = permissionChecker;
            _logger = logger;
            //BtnPerms = new Dictionary<string, Visibility>
            //{
            //    { nameof(IdentityPermissions.Users.Create), Visibility.Hidden },
            //    { nameof(IdentityPermissions.Users.Update), Visibility.Hidden },
            //    { nameof(IdentityPermissions.Users.ManagePermissions), Visibility.Hidden },
            //    { nameof(IdentityPermissions.Users.Delete), Visibility.Hidden },
            //};
        }

        internal async Task InitialAsync(UserView owner)
        {
            var loadDialog = Dialog.Show<LoadingCircle>(DialogContainerToken);
            try
            {
                Owner = owner;

                var result = await _permissionChecker.IsGrantedAsync(new string[]
                {
                    IdentityPermissions.Users.Create,
                    IdentityPermissions.Users.Update,
                    IdentityPermissions.Users.ManagePermissions,
                    IdentityPermissions.Users.Delete,
                });

                var result2 = await _permissionChecker.IsGrantedAsync(IdentityPermissions.Users.Create);
                await LoadDataAsync();
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
        private async Task SearchAsync()
        {
            var loadDialog = Dialog.Show<LoadingCircle>(DialogContainerToken);
            try
            {
                await LoadDataAsync();
            }
            catch (AbpValidationException abpEx)
            {
                _logger.LogException(abpEx);
                var errorMessages = abpEx.ValidationErrors.Select(x => x.ErrorMessage);
                MessageBox.Error(string.Join('.', errorMessages), abpEx.Message);
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
        private void Reset()
        {
            Name = string.Empty;
        }

        [RelayCommand]
        private async Task AddAsync()
        {
            try
            {
                var view = new UserAddView();
                var result = view.ShowDialog();
                if (result == true)
                {
                    Growl.Success("新增成功");
                    await SearchCommand.ExecuteAsync(null);
                }
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                MessageBox.Error(ex.Message);
            }
        }

        [RelayCommand]
        private async Task EditAsync(IdentityUserDto user)
        {
            try
            {
                var view = new UserEditView(user.Id);
                var result = view.ShowDialog();
                if (result == true)
                {
                    Growl.Success("更新成功");
                    await SearchCommand.ExecuteAsync(null);
                }
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                MessageBox.Error(ex.Message);
            }
        }

        [RelayCommand]
        private void EditPerm(IdentityUserDto user)
        {
            var view = new PermissionEditView("U", user.UserName);
            var result = view.ShowDialog();
            if (result == true)
            {
                Growl.Success("更新权限成功");
            }
        }

        [RelayCommand]
        private async Task Delete(IdentityUserDto user)
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
                await _identityUserAppService.DeleteAsync(user.Id);
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
                var userIds = sender.Cast<IdentityUserDto>().Select(x => x.Id).ToList();
                if (userIds.Count == 0)
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
                foreach (var userId in userIds)
                {
                    await _identityUserAppService.DeleteAsync(userId);
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

        [RelayCommand]
        private async Task PageChangedAsync(Tuple<int, int> pageArgs)
        {
            PageIndex = pageArgs.Item1;
            PageSize = pageArgs.Item2;
            await SearchCommand.ExecuteAsync(null);
        }

        private async Task LoadDataAsync()
        {
            var result = await _identityUserAppService.GetListAsync(new GetIdentityUsersInput
            {
                Filter = Name,
                SkipCount = (PageIndex - 1) * PageSize,
                MaxResultCount = PageSize
            });
            TotalCount = result.TotalCount;
            Users = new ObservableCollection<IdentityUserDto>(result.Items);
        }
    }
}
