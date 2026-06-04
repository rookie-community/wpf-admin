using Admin.Commons;
using Admin.Identity;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Admin;

public class AdminStoreDataSeederContributor : IDataSeedContributor, ITransientDependency
{
    public const string AdminEmailPropertyName = "AdminEmail";
    public const string AdminEmailDefaultValue = "admin@abp.io";
    public const string AdminUserNamePropertyName = "AdminUserName";
    public const string AdminUserNameDefaultValue = "admin";
    public const string AdminPasswordPropertyName = "AdminPassword";
    public const string AdminPasswordDefaultValue = "1q2w3E*";

    private readonly IRepository<User, Guid> _userRepository;
    private readonly IRepository<Role, Guid> _roleRepository;
    private readonly IRepository<UserRole> _userroleRepository;

    public AdminStoreDataSeederContributor(
        IRepository<User, Guid> userRepository,
        IRepository<Role, Guid> roleRepository,
        IRepository<UserRole> userroleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _userroleRepository = userroleRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        if (await _userRepository.GetCountAsync() > 0)
        {
            return;
        }
        var role = await _roleRepository.FirstOrDefaultAsync();
        if (role == null)
        {
            role = await _roleRepository.InsertAsync(new Role
            {
                Name = "Admin",
                IsDefault = false
            });
        }

        var user = await _userRepository.InsertAsync(new User
        {
            Account = AdminUserNameDefaultValue,
            UserName = AdminUserNameDefaultValue,
            Password = MD5Helper.GetMD5(AdminPasswordDefaultValue),
            IsActive = true,
            Email = AdminEmailDefaultValue,
        });

        await _userroleRepository.InsertAsync(new UserRole
        {
            UserId = user.Id,
            RoleId = role.Id
        });
    }


}
