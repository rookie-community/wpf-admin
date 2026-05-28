using Admin.EntityFrameworkCore.EntityFrameworkCore;
using Admin.Identity;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Admin.EntityFrameworkCore.Identity
{
    public class EfCoreUserRepository : EfCoreRepository<AdminEfCoreDbContext, User, Guid>, IUserRepository
    {
        public EfCoreUserRepository(IDbContextProvider<AdminEfCoreDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
