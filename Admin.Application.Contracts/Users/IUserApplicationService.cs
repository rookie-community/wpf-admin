using Volo.Abp.Application.Services;

namespace Admin.Users
{
    public interface IUserApplicationService : IApplicationService
    {
        Task<LoginResultDto> LoginAsunc(LoginDto input);

        Task<LoginResultDto> RefreshTokenAsync(Guid refreshToken);
    }
}
