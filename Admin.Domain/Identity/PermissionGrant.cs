using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Admin.Identity
{
    /// <summary>
    /// 权限关联表
    /// </summary>
    public class PermissionGrant : AggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public string Name { get; set; } = null!;

        /// <summary>
        /// Role,User,Organization
        /// </summary>
        public string ProviderName { get; set; } = null!;

        public string ProviderKey { get; set; } = null!;
    }
}
