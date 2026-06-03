using System.Linq.Expressions;
using Volo.Abp.Domain.Repositories;

namespace Admin.Identity
{
    public interface IUserRepository : IRepository<User, Guid>
    {
        Task<List<User>> GetPagedListAsync(int skipCount, int maxResultCount, Expression<Func<User, bool>> predicate);

        Task<long> CountAsync(Expression<Func<User, bool>> predicate);
    }
}
