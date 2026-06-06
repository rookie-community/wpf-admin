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
    public partial class EditUserPasswordVM : ObservableValidator, ITransientDependency
    {
        [Required]
        [ObservableProperty]
        private string oldPassword = null!;

        [Required(ErrorMessage = "请输入新密码")]
        [MinLength(6, ErrorMessage = "密码长度至少6位")]
        [ObservableProperty]
        private string newPassword = null!;

        [Required]
        [CustomValidation(typeof(EditUserPasswordVM), nameof(ValidateConfirmPassword))]
        [ObservableProperty]
        private string confirmPassword = null!;

        [ObservableProperty]
        private string dialogContainerToken = Guid.NewGuid().ToString();

        public EditUserPasswordView Owner { get; private set; } = null!;

        private readonly IUserApplicationService _userApplicationService;
        private readonly ILogger<EditUserPasswordVM> _logger;

        public EditUserPasswordVM(IUserApplicationService userApplicationService, ILogger<EditUserPasswordVM> logger)
        {
            _userApplicationService = userApplicationService;
            _logger = logger;
        }

        internal void Initial(EditUserPasswordView owner)
        {
            Owner = owner;
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
                await _userApplicationService.UpdateCurrentUserPasswordAsync(new UpdateCurrentPasswordDto
                {
                    Id = App.CurrentUser.Id,
                    OldPassword = OldPassword,
                    NewPassword = NewPassword,
                });
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

        public static ValidationResult ValidateConfirmPassword(string confirmPasswordValue, ValidationContext context)
        {
            var instance = (EditUserPasswordVM)context.ObjectInstance;
            if (confirmPasswordValue != instance.NewPassword)
            {
                return new ValidationResult("两次输入的密码不一致！");
            }
            return ValidationResult.Success!;
        }
    }
}
