using Admin.EntityFrameworkCore.EntityFrameworkCore;
using Admin.Identity;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Admin.EntityFrameworkCore.Identity
{
    public class EfCorePermissionRepository : EfCoreRepository<AdminEfCoreDbContext, Permission, Guid>, IPermissionRepository
    {
        public EfCorePermissionRepository(IDbContextProvider<AdminEfCoreDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
