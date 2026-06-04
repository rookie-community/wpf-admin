using Admin.Commons;
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
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Security.Claims;
using Volo.Abp.TenantManagement;

namespace Admin.Users
{
    public class UserApplicationService : AdminAppService, IUserApplicationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRepository<Tenant, Guid> _tenantRepository;
        private readonly IDistributedCache<string> _distributedCache;
        private readonly IConfiguration _configuration;

        public UserApplicationService(IUserRepository userRepository, IRepository<Tenant, Guid> tenantRepository, IDistributedCache<string> distributedCache, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _tenantRepository = tenantRepository;
            _distributedCache = distributedCache;
            _configuration = configuration;
        }

        public async Task<TokenResultDto> LoginAsync(LoginDto input)
        {
            var md5Password = MD5Helper.GetMD5(input.Password);
            var user = await _userRepository.FirstOrDefaultAsync(x => x.Account == input.Account && x.Password == md5Password);
            if (user == null)
            {
                throw new AbpAuthorizationException("用户不存在");
            }
            return await GenerateToken(user);
        }

        [Authorize]
        public async Task<CurrentUserDto> GetCurrentUserInfoAsync()
        {
            var user = await _userRepository.FirstOrDefaultAsync(x => x.Id == CurrentUser.Id);
            if (user == null)
            {
                throw new AbpAuthorizationException("获取用户数据失败！");
            }

            var userDto = new CurrentUserDto
            {
                Id = user.Id,
                Account = user.Account,
                UserName = user.UserName,
                IsActive = user.IsActive,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
            };

            if (user.TenantId != default)
            {
                var tenant = await _tenantRepository.FirstOrDefaultAsync(x => x.Id == user.TenantId);
                userDto.TenantName = tenant?.Name ?? string.Empty;
            }

            return userDto;
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

            var userId = CurrentUser.Id ?? Guid.Empty;
            var user = await _userRepository.GetAsync(userId);
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
                new Claim(AbpClaimTypes.UserId, user.Id.ToString()),
                new Claim(AbpClaimTypes.Name, user.UserName),
                new Claim(AbpClaimTypes.UserName, user.UserName),
                new Claim(AbpClaimTypes.SurName, user.UserName),
                new Claim(AbpClaimTypes.PhoneNumber, user.PhoneNumber ?? string.Empty),
                new Claim(AbpClaimTypes.Email, user.Email),
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
                filter = filter.And(x => x.UserName.Contains(model.UserName));
            }

            if (!string.IsNullOrWhiteSpace(model.PhoneNumber))
            {
                filter = filter.And(x => x.PhoneNumber.Equals(model.PhoneNumber));
            }

            if (!string.IsNullOrWhiteSpace(model.Email))
            {
                filter = filter.And(x => x.Email.Equals(model.Email));
            }

            return filter;
        }
    }
}
