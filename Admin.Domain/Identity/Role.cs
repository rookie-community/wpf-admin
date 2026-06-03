using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Admin.Identity
{
    /// <summary>
    /// 角色表
    /// </summary>
    public class Role : Entity<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }
        public string Name { get; set; } = null!;

        public bool IsDefault { get; set; }
    }
}
