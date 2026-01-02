using Admin.EntityFrameworkCore.EntityFrameworkCore;
using Admin.Identity;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Admin.EntityFrameworkCore.Identity
{
    public class UserRepository : EfCoreRepository<AdminEfCoreDbContext, User, Guid>, IUserRepository
    {
        public UserRepository(IDbContextProvider<AdminEfCoreDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
