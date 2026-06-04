using Admin.Desktop.Resources.Langs;
using Admin.Desktop.Tools;
using Admin.Desktop.View;
using Admin.Desktop.View.Accounts;
using Admin.Users;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FastReport.Utils;
using HandyControl.Tools;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using Volo.Abp.DependencyInjection;
using MessageBox = HandyControl.Controls.MessageBox;

namespace Admin.Desktop.ViewModel.Accounts
{
    public partial class LoginVM : ObservableValidator, ITransientDependency
    {
        [ObservableProperty]
        [CustomValidation(typeof(LoginVM), nameof(ValidateTenant))]
        private string tenant = null!;

        [Required(ErrorMessage = "账号不能为空")]
        [ObservableProperty]
        private string userName = null!;

        [Required(ErrorMessage = "密码不能为空")]
        [MinLength(6, ErrorMessage = "密码长度至少6位")]
        [ObservableProperty]
        private string password = null!;

        [ObservableProperty]
        private IReadOnlyDictionary<string, string> langItems = new Dictionary<string, string>();

        [ObservableProperty]
        private string currentLang = LangProvider.Culture.Name;

        [ObservableProperty]
        private bool isUploading;
        private readonly IUserApplicationService _userApplicationService;
        private readonly ILogger<LoginVM> _logger;

        public Login Owner { get; private set; } = null!;

        public LoginVM(IUserApplicationService userApplicationService, ILogger<LoginVM> logger)
        {
            _userApplicationService = userApplicationService;
            _logger = logger;
        }

        public void Initial(Login owner)
        {
            Owner = owner;
            if (Debugger.IsAttached)
            {
                UserName = "admin";
                Password = "1q2w3E*";
            }
            LangItems = new Dictionary<string, string>
            {
                { "简体中文", "zh-CN"},
                // 英语本土名称
                { "English", "en" },  
                // 波斯语（法尔西语）本土名称
                { "فارسی", "fa" },   
                // 法语本土名称
                { "Français", "fr" },   
                // 加泰罗尼亚语本土名称（西班牙加泰罗尼亚）
                { "Català", "ca-ES" },    
                // 日语本土名称
                { "日本語", "ja" },  
                // 韩语（朝鲜语）本土名称
                { "한국어", "ko-KR" },
                // 俄语本土名称
                { "Русский", "ru" }, 
                // 土耳其语本土名称
                { "Türkçe", "tr" },         
                // 巴西葡萄牙语本土名称
                { "Português (Brasil)", "pt-BR" },
                // 波兰语本土名称
                { "Polski", "pl" },
                // 西班牙语本土名称
                { "Español", "es" },
            };
        }

        [RelayCommand]
        private async Task SubmitAsync()
        {
            try
            {
                ValidateAllProperties();
                if (HasErrors)
                {
                    return;
                }

                var tokenResult = await _userApplicationService.LoginAsync(new LoginDto
                {
                    Account = UserName,
                    Password = Password,
                    Tenant = Tenant,
                });
                TokenManager.SetTokens(tokenResult.AccessToken, tokenResult.RefreshToken);
                var userInfo = await _userApplicationService.GetCurrentUserInfoAsync();
                App.SetCurrentUser(userInfo);
                var view = new MainWindow();
                view.Show();
                Owner.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Error(ex.Message, "登录失败");
                _logger.LogError(ex.Message);
            }
            finally
            {
                IsUploading = false;
            }
        }

        [RelayCommand]
        private void Cancel()
        {
            Owner.Close();
        }

        partial void OnCurrentLangChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                //更新语言
                ConfigHelper.Instance.SetLang(value);
                LangProvider.Culture = new CultureInfo(value);
                Res.LoadLocale(LangProvider.Culture);
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["Language"].Value = value;
                config.Save(ConfigurationSaveMode.Modified);
            }
        }

        public static ValidationResult ValidateTenant(string tenantValue, ValidationContext context)
        {
            var instance = (LoginVM)context.ObjectInstance;
            if (string.IsNullOrWhiteSpace(tenantValue))
            {
                //租户为空，无需校验
                return ValidationResult.Success!;
            }
            //校验租户是否存在
            //todo
            return new ValidationResult("租户功能暂未开放！");
        }
    }
}
