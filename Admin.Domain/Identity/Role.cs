using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Admin.Identity
{
    /// <summary>
    /// 角色表
    /// </summary>
    public class Role : AggregateRoot<Guid>, IMultiTenant
    {
        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public Guid? TenantId { get; set; }
    }
}
