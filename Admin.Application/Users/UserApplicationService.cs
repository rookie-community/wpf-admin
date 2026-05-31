using Admin.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Volo.Abp.Security.Claims;
using Volo.Abp.Users;

namespace Admin.Users
{
    public class UserApplicationService : AdminAppService, IUserApplicationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IDistributedCache<string> _distributedCache;
        private readonly IConfiguration _configuration;

        public UserApplicationService(ICurrentUser currentUser, IUserRepository userRepository, IDistributedCache<string> distributedCache, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _distributedCache = distributedCache;
            _configuration = configuration;
        }

        public async Task<LoginResultDto> LoginAsync(LoginDto input)
        {
            //var user = await _userRepository.FindAsync(x => x.Account == input.Account && x.Password == input.Password);
            //if (user == null)
            //{
            //    throw new AbpAuthorizationException("用户不存在");
            //}
            var user = new User
            {
                Account = input.Account,
                UserName = "Admin",
                Password = input.Password
            };
            return await GenerateToken(user);
        }

        public async Task<LoginResultDto> RefreshTokenAsync(Guid refreshToken)
        {
            var cacheToken = _distributedCache.Get(refreshToken.ToString());
            if (string.IsNullOrEmpty(cacheToken))
            {
                throw new AbpAuthorizationException("无效的刷新令牌");
            }
            // 创建一个JWT安全令牌处理器
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            //验证Token格式
            if (!jwtSecurityTokenHandler.CanReadToken(cacheToken))
            {
                throw new AbpAuthorizationException("访问令牌格式错误");
            }

            //var userId = Guid.NewGuid();
            //var user = await _userRepository.GetAsync(userId);
            var user = new User
            {
                Account = "Admin",
                UserName = "Admin",
            };
            return await GenerateToken(user);
        }

        private async Task<LoginResultDto> GenerateToken(User user)
        {
            var jwtConfig = _configuration.GetSection("Authentication:JwtBearer");
            var securityKey = jwtConfig["SecurityKey"]!;
            var issuer = jwtConfig["Issuer"];
            var audience = jwtConfig["Audience"];
            int expires = jwtConfig.GetValue<int>("ExpirationTime");
            var claims = new List<Claim>
            {
                //new Claim(JwtRegisteredClaimNames.Nbf, DateTimeOffset.Now.ToUnixTimeSeconds().ToString()) ,
                //new Claim (JwtRegisteredClaimNames.Exp, DateTimeOffset.Now.AddMinutes(expires).ToUnixTimeSeconds().ToString()),
                //new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(AbpClaimTypes.UserId, Guid.NewGuid().ToString()),
                new Claim(AbpClaimTypes.Name, user.UserName),
                new Claim(AbpClaimTypes.UserName, user.UserName),
                new Claim(AbpClaimTypes.SurName, user.UserName),
                new Claim(AbpClaimTypes.PhoneNumber, "1234567"),
                new Claim(AbpClaimTypes.Email, "测试Email"),
                new Claim(AbpClaimTypes.Role, "测试Role"),
                new Claim(AbpClaimTypes.TenantId, Guid.NewGuid().ToString()),
            };

            //sign the token using a secret key.This secret will be shared between your API and anything that needs to check that the token is legit.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //.NET Core’s JwtSecurityToken class takes on the heavy lifting and actually creates the token.

            var token = new JwtSecurityToken(
                //颁发者
                issuer,
                //接收者
                audience,
                //自定义参数
                claims,
                notBefore: DateTime.Now,
                //过期时间
                expires: DateTime.Now.AddMinutes(expires),
                //签名证书
                signingCredentials: creds
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = Guid.NewGuid();
            var result = new LoginResultDto
            {
                Id = user.Id,
                Account = user.Account,
                UserName = user.UserName,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1),
            };
            await _distributedCache.SetAsync(refreshToken.ToString(), accessToken, options);
            return result;
        }
    }
}
