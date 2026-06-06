using Admin.Desktop.View.Users;
using Admin.Users;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Controls;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.DependencyInjection;
using MessageBox = HandyControl.Controls.MessageBox;

namespace Admin.Desktop.ViewModel.Users
{
    public partial class UserInfoVM : ObservableValidator, ITransientDependency
    {
        [ObservableProperty]
        private string tenantName = string.Empty;

        [ObservableProperty]
        private string account = string.Empty;

        [Required]
        [ObservableProperty]
        private string? userName;

        [ObservableProperty]
        private bool isActive;

        [ObservableProperty]
        private string? phoneNumber;

        [ObservableProperty]
        private string? email;

        [ObservableProperty]
        private string dialogContainerToken = Guid.NewGuid().ToString();

        private readonly IUserApplicationService _userApplicationService;
        private readonly ILogger<UserInfoVM> _logger;
        private CurrentUserDto _currentUser = null!;
        public UserInfoView Owner { get; private set; } = null!;

        public UserInfoVM(IUserApplicationService userApplicationService, ILogger<UserInfoVM> logger)
        {
            _userApplicationService = userApplicationService;
            _logger = logger;
        }

        internal async Task InitialAsync(UserInfoView owner)
        {
            var loadDialog = Dialog.Show(new LoadingCircle(), DialogContainerToken);
            try
            {
                Owner = owner;
                var user = await _userApplicationService.GetCurrentUserInfoAsync();
                _currentUser = user;
                SetData(user);
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
        private async Task SubmitAsync()
        {
            var loadDialog = Dialog.Show(new LoadingCircle(), DialogContainerToken);
            try
            {
                ValidateAllProperties();
                if (HasErrors)
                {
                    return;
                }
                await _userApplicationService.UpdateCurrentUserAsync(new UpdateCurrentUserDto
                {
                    Id = _currentUser.Id,
                    UserName = UserName,
                    PhoneNumber = PhoneNumber,
                    Email = Email
                });

                //修改成功后，更新本地数据
                var user = await _userApplicationService.GetCurrentUserInfoAsync();
                App.SetCurrentUser(user);
                MessageBox.Success("修改成功");
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
            SetData(_currentUser);
        }

        private void SetData(CurrentUserDto user)
        {
            if (user == null)
            {
                return;
            }

            TenantName = user.TenantName;
            Account = user.Account;
            UserName = user.UserName;
            IsActive = user.IsActive;
            PhoneNumber = user.PhoneNumber;
            Email = user.Email;
        }
    }
}
