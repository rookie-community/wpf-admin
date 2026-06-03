using Admin.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Volo.Abp.Security.Claims;

namespace Admin.Users
{
    public class UserApplicationService : AdminAppService, IUserApplicationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IDistributedCache<string> _distributedCache;
        private readonly IConfiguration _configuration;

        public UserApplicationService(IUserRepository userRepository, IDistributedCache<string> distributedCache, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _distributedCache = distributedCache;
            _configuration = configuration;
        }

        public async Task<TokenResultDto> LoginAsync(LoginDto input)
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

        [Authorize]
        public Task<CurrentUserDto> GetCurrentUserInfoAsync()
        {
            var userDto = new CurrentUserDto
            {
                Id = CurrentUser.Id ?? Guid.Empty,
                TenantId = CurrentUser.TenantId,
                UserName = CurrentUser.UserName ?? string.Empty,
                PhoneNumber = CurrentUser.PhoneNumber,
                Email = CurrentUser.Email ?? string.Empty,
            };
            return Task.FromResult(userDto);
        }

        public async Task<TokenResultDto> RefreshTokenAsync(Guid refreshToken)
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

        private async Task<TokenResultDto> GenerateToken(User user)
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

            var expiresTime = TimeSpan.FromMinutes(expires);
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
                expires: DateTime.Now.Add(expiresTime),
                //签名证书
                signingCredentials: creds
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = Guid.NewGuid();
            var result = new TokenResultDto
            {
                Id = user.Id,
                UserName = user.UserName,
                TokenType = "Bearer",
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = expiresTime.TotalSeconds
            };

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1),
            };
            await _distributedCache.SetAsync(refreshToken.ToString(), accessToken, options);
            return result;
        }

        public async Task<PagedResultDto<UserDto>> GetListAsync(GetUserListDto input)
        {
            if (input.Sorting.IsNullOrWhiteSpace())
            {
                input.Sorting = nameof(User.Id);
            }

            var predicate = GetListFilter(input);
            var authors = await _userRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, predicate);
            var totalCount = await _userRepository.CountAsync(predicate);
            return new PagedResultDto<UserDto>(totalCount, ObjectMapper.Map<List<User>, List<UserDto>>(authors));
        }

        private Expression<Func<User, bool>> GetListFilter(GetUserListDto model)
        {
            var filter = PredicateBuilder.New<User>(true);
            if (!string.IsNullOrWhiteSpace(model.UserName))
            {
                filter = filter.And(x => x.UserName.Equals(model.UserName));
            }
            return filter;
        }
    }
}
