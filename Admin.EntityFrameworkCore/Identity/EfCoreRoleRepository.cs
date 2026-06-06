using Admin.EntityFrameworkCore.EntityFrameworkCore;
using Admin.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Admin.EntityFrameworkCore.Identity
{
    internal class EfCoreRoleRepository : EfCoreRepository<AdminEfCoreDbContext, Role, Guid>, IRoleRepository
    {
        public EfCoreRoleRepository(IDbContextProvider<AdminEfCoreDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<long> CountAsync(Expression<Func<Role, bool>> predicate)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.Where(predicate).CountAsync();
        }

        public async Task<List<Role>> GetPagedListAsync(int skipCount, int maxResultCount, Expression<Func<Role, bool>> predicate)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.Where(predicate).OrderByDescending(x => x.Id).Skip(skipCount).Take(maxResultCount).ToListAsync();
        }
    }
}
