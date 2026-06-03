using Admin.EntityFrameworkCore.EntityFrameworkCore;
using Admin.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Admin.EntityFrameworkCore.Identity
{
    public class EfCoreUserRepository : EfCoreRepository<AdminEfCoreDbContext, User, Guid>, IUserRepository
    {
        public EfCoreUserRepository(IDbContextProvider<AdminEfCoreDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<long> CountAsync(Expression<Func<User, bool>> predicate)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.Where(predicate).CountAsync();
        }

        public async Task<List<User>> GetPagedListAsync(int skipCount, int maxResultCount, Expression<Func<User, bool>> predicate)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.Where(predicate).OrderByDescending(x => x.Id).Skip(skipCount).Take(maxResultCount).ToListAsync();
        }
    }
}
