using Admin.Desktop.View.Users;
using Admin.Users;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.DependencyInjection;
using MessageBox = HandyControl.Controls.MessageBox;

namespace Admin.Desktop.ViewModel.Users
{
    public partial class UserInfoVM : ObservableValidator, ITransientDependency
    {
        [ObservableProperty]
        private string tenantName;

        [ObservableProperty]
        private string account;

        [Required]
        [ObservableProperty]
        private string? userName;

        [ObservableProperty]
        private bool isActive;

        [ObservableProperty]
        private string? phoneNumber;

        [ObservableProperty]
        private string? email;
        private readonly IUserApplicationService _userApplicationService;
        private readonly ILogger<UserInfoVM> _logger;

        public UserInfoView Owner { get; private set; } = null!;

        public UserInfoVM(IUserApplicationService userApplicationService, ILogger<UserInfoVM> logger)
        {
            _userApplicationService = userApplicationService;
            _logger = logger;
        }

        internal void Initial(UserInfoView owner)
        {
            Owner = owner;
            InitialUser();
        }

        private void InitialUser()
        {
            var user = App.CurrentUser;
            TenantName = user.TenantName;
            Account = user.Account;
            UserName = user.UserName;
            IsActive = user.IsActive;
            PhoneNumber = user.PhoneNumber;
            Email = user.Email;
        }

        [RelayCommand]
        private async Task SubmitAsync()
        {
            ValidateAllProperties();
            if (HasErrors)
            {
                return;
            }
            MessageBox.Success("修改成功");
        }

        [RelayCommand]
        private void Reset()
        {
            InitialUser();
        }
    }
}
