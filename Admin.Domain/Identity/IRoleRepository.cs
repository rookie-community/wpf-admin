using System.Linq.Expressions;
using Volo.Abp.Domain.Repositories;

namespace Admin.Identity
{
    public interface IRoleRepository : IRepository<Role, Guid>
    {
        Task<List<Role>> GetPagedListAsync(int skipCount, int maxResultCount, Expression<Func<Role, bool>> predicate);

        Task<long> CountAsync(Expression<Func<Role, bool>> predicate);
    }
}
