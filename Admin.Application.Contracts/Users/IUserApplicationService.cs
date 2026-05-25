using Volo.Abp.Application.Services;

namespace Admin.Users
{
    public interface IUserApplicationService : IApplicationService
    {
        Task<LoginResultDto> Login(LoginDto input);
    }
}
