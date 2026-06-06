using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Admin.Users
{
    public interface IUserApplicationService : IApplicationService
    {
        Task<TokenResultDto> LoginAsync(LoginDto input);

        Task<TokenResultDto> RefreshTokenAsync(Guid refreshToken);

        Task<CurrentUserDto> GetCurrentUserInfoAsync();

        Task<PagedResultDto<UserDto>> GetListAsync(GetUserListDto input);

        Task UpdateCurrentUserPasswordAsync(UpdateCurrentPasswordDto input);

        Task UpdateCurrentUserAsync(UpdateCurrentUserDto input);

        Task BatchDelete(IList<Guid> userIds);
    }
}
