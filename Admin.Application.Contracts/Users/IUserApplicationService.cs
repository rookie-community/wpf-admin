using Volo.Abp.Application.Services;

namespace Admin.Users
{
    public interface IUserApplicationService : IApplicationService
    {
        Task<LoginResultDto> LoginAsync(LoginDto input);

        Task<LoginResultDto> RefreshTokenAsync(Guid refreshToken);
    }
}
