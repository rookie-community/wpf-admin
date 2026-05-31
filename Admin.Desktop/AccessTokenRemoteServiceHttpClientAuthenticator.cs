using Admin.Desktop.Tools;
using Admin.Users;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.Authentication;

namespace Admin.Desktop
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IRemoteServiceHttpClientAuthenticator))]
    public class AccessTokenRemoteServiceHttpClientAuthenticator : IRemoteServiceHttpClientAuthenticator, ITransientDependency
    {
        private readonly IUserApplicationService _userApplicationService;
        private readonly ILogger<AccessTokenRemoteServiceHttpClientAuthenticator> _logger;

        public AccessTokenRemoteServiceHttpClientAuthenticator(IUserApplicationService userApplicationService, ILogger<AccessTokenRemoteServiceHttpClientAuthenticator> logger)
        {
            _userApplicationService = userApplicationService;
            _logger = logger;
        }

        public async Task Authenticate(RemoteServiceHttpClientAuthenticateContext context)
        {
            // 获取token
            var currentAccessToken = TokenManager.AccessToken;
            // 判断Token是否过期
            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(currentAccessToken))
            {
                return;
            }
            var jwtToken = handler.ReadToken(currentAccessToken);
            // 直接使用ValidTo属性，它代表Token的过期时间
            // 注意：ValidTo是DateTime类型，但通常是UTC时间
            if (DateTime.UtcNow > jwtToken.ValidTo)
            {
                var result = await _userApplicationService.RefreshTokenAsync(TokenManager.RefreshToken);
                if (result == null)
                {
                    _logger.LogError("刷新令牌失败");
                    // 导向用户重新登录的页面...
                    return;
                }
                TokenManager.SetTokens(result.AccessToken, result.RefreshToken);
                currentAccessToken = TokenManager.AccessToken;
            }
            context.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", currentAccessToken);
        }
    }
}
