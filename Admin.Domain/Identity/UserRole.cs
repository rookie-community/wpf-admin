using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Admin.Identity
{
    /// <summary>
    /// 用户角色关联表
    /// </summary>
    public class UserRole : Entity, IMultiTenant
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public Guid? TenantId { get; set; }

        public override object[] GetKeys()
        {
            return new object[] { UserId, RoleId };
        }
    }
}
