using Volo.Abp.Application.Services;

namespace Admin.Permissions
{
    public interface IPermissionApplicationService : IApplicationService
    {
        Task<string> Test();
    }
}
