using Admin.Desktop.View.SettingManagement.EmailSettings;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Controls;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.SettingManagement;
using MessageBox = HandyControl.Controls.MessageBox;

namespace Admin.Desktop.ViewModel.SettingManagement.EmailSettings
{
    public partial class EmailSettingVM : ObservableRecipient, ITransientDependency
    {
        private readonly IEmailSettingsAppService _emailSettingsAppService;
        private readonly ILogger<EmailSettingVM> _logger;

        [ObservableProperty]
        public partial EmailSettingsDto EmailSettings { get; set; } = null!;

        [ObservableProperty]
        public partial string DialogContainerToken { get; set; } = Guid.NewGuid().ToString();

        public EmailSettingView Owner { get; private set; } = null!;

        public EmailSettingVM(IEmailSettingsAppService emailSettingsAppService, ILogger<EmailSettingVM> logger)
        {
            _emailSettingsAppService = emailSettingsAppService;
            _logger = logger;
        }

        internal async Task InitialAsync(EmailSettingView owner)
        {
            var loadDialog = Dialog.Show(new LoadingCircle(), DialogContainerToken);
            try
            {
                Owner = owner;
                EmailSettings = await _emailSettingsAppService.GetAsync();
                await Task.Delay(1);
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
        public void SendTestEmail()
        {
            var view = new SendTestEmailView();
            var result = view.ShowDialog();
            if (result == true)
            {
                Growl.Success("发送成功");
            }
        }

        [RelayCommand]
        public async Task SubmitAsync()
        {
            var loadDialog = Dialog.Show(new LoadingCircle(), DialogContainerToken);
            try
            {
                await _emailSettingsAppService.UpdateAsync(new UpdateEmailSettingsDto
                {
                    DefaultFromDisplayName = EmailSettings.DefaultFromDisplayName,
                    DefaultFromAddress = EmailSettings.DefaultFromAddress,
                    SmtpHost = EmailSettings.SmtpHost,
                    SmtpPort = EmailSettings.SmtpPort,
                    SmtpEnableSsl = EmailSettings.SmtpEnableSsl,
                    SmtpUseDefaultCredentials = EmailSettings.SmtpUseDefaultCredentials,
                });
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
    }
}
