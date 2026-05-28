using Volo.Abp.Domain.Repositories;

namespace Admin.Identity
{
    public interface IPermissionRepository : IRepository<Permission, Guid>
    {
    }
}
