using Volo.Abp.Domain.Repositories;

namespace Admin.Identity
{
    public interface IUserRepository : IRepository<User, Guid>
    {
    }
}
