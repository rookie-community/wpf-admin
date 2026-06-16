using Admin.Desktop.View.SettingManagement.EmailSettings;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Controls;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.SettingManagement;

namespace Admin.Desktop.ViewModel.SettingManagement.EmailSettings
{
    public partial class SendTestEmailVM : ObservableRecipient, ITransientDependency
    {
        private readonly IEmailSettingsAppService _emailSettingsAppService;
        private readonly ILogger<SendTestEmailVM> _logger;

        [ObservableProperty]
        public partial SendTestEmailInput EmailInput { get; set; } = null!;

        [ObservableProperty]
        public partial string DialogContainerToken { get; set; } = Guid.NewGuid().ToString();

        public SendTestEmailView Owner { get; private set; } = null!;

        public SendTestEmailVM(IEmailSettingsAppService emailSettingsAppService, ILogger<SendTestEmailVM> logger)
        {
            _emailSettingsAppService = emailSettingsAppService;
            _logger = logger;
        }

        internal void Initial(SendTestEmailView owner)
        {
            Owner = owner;
            EmailInput = new SendTestEmailInput
            {
                SenderEmailAddress = "noreply@abp.io",
                TargetEmailAddress = "admin@abp.io",
                Subject = "测试邮件 6794",
                Body = "测试邮件内容"
            };
        }

        [RelayCommand]
        public async Task SubmitAsync()
        {
            var loadDialog = Dialog.Show<LoadingCircle>(DialogContainerToken);
            try
            {
                await _emailSettingsAppService.SendTestEmailAsync(EmailInput);
                Owner.DialogResult = true;
                Owner.Close();
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
        public void Cancel()
        {
            Owner.DialogResult = false;
            Owner.Close();
        }
    }
}
