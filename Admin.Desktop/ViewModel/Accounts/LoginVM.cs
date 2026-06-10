using Admin.Desktop.Resources.Langs;
using Admin.Desktop.Tools;
using Admin.Desktop.View;
using Admin.Desktop.View.Accounts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Duende.IdentityModel.Client;
using FastReport.Utils;
using HandyControl.Tools;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using MessageBox = HandyControl.Controls.MessageBox;

namespace Admin.Desktop.ViewModel.Accounts
{
    public partial class LoginVM : ObservableValidator, ITransientDependency
    {
        [ObservableProperty]
        [CustomValidation(typeof(LoginVM), nameof(ValidateTenant))]
        public partial string Tenant { get; set; } = null!;

        [Required(ErrorMessage = "账号不能为空")]
        [ObservableProperty]
        public partial string UserName { get; set; } = null!;

        [Required(ErrorMessage = "密码不能为空")]
        [MinLength(6, ErrorMessage = "密码长度至少6位")]
        [ObservableProperty]
        public partial string Password { get; set; } = null!;

        [ObservableProperty]
        public partial IReadOnlyDictionary<string, string> LangItems { get; set; } = new Dictionary<string, string>();

        [ObservableProperty]
        public partial string CurrentLang { get; set; } = LangProvider.Culture.Name;

        [ObservableProperty]
        public partial bool IsUploading { get; set; }

        private readonly IIdentityUserAppService _userAppService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<LoginVM> _logger;

        public Login Owner { get; private set; } = null!;

        public LoginVM(IIdentityUserAppService userAppService, IHttpClientFactory httpClientFactory, ILogger<LoginVM> logger)
        {
            _userAppService = userAppService;
            _httpClientFactory = httpClientFactory;
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
                { "English", "en" }
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

                using var httpClient = _httpClientFactory.CreateClient();
                var identityServerUrl = ConfigurationManager.AppSettings["RemoteServices"];
                // 1. 获取发现文档
                var disco = await httpClient.GetDiscoveryDocumentAsync(identityServerUrl);
                if (disco.IsError)
                {
                    MessageBox.Error($"Failed to get discovery document: {disco.Error}", "登录失败");
                    return;
                }

                // 2. 请求 token
                var tokenRequest = new PasswordTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    ClientId = ConfigurationManager.AppSettings["ClientId"] ?? string.Empty,
                    ClientSecret = null,//客户端密钥
                    UserName = UserName,
                    Password = Password,
                    //offline_access - 请求长期访问权限
                    Scope = "Admin"// offline_access" //"YourApp"
                };

                var tokenResponse = await httpClient.RequestPasswordTokenAsync(tokenRequest);
                if (tokenResponse.IsError)
                {
                    MessageBox.Error($"Failed to get token: {tokenResponse.Error} - {tokenResponse.ErrorDescription}", "登录失败");
                    _logger.LogError("Failed to get token: {Error} - {ErrorDescription}", tokenResponse.Error, tokenResponse.ErrorDescription);
                    return;
                }


                var handler = new JwtSecurityTokenHandler();
                if (!handler.CanReadToken(tokenResponse.AccessToken))
                {
                    MessageBox.Error("无法解析 JWT 字符串");
                    return;
                }
                TokenManager.SetTokens(tokenResponse);

                var jwtSecurityToken = handler.ReadJwtToken(tokenResponse.AccessToken);
                _ = Guid.TryParse(jwtSecurityToken.Subject, out var userId);
                var user = await _userAppService.GetAsync(userId);
                App.SetCurrentUser(user);
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
