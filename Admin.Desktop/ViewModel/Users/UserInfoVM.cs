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
        private Guid? tenantId;

        [Required]
        [ObservableProperty]
        private string? userName;

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
            TenantId = user.TenantId;
            UserName = user.UserName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
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
