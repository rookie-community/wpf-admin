using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Admin.Roles
{
    public interface IRoleApplicationService : IApplicationService
    {
        Task<PagedResultDto<RoleDto>> GetListAsync(GetRoleListDto input);

        Task BatchDelete(IList<Guid> roleIds);
    }
}
