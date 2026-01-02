using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace Admin.Identity
{
    /// <summary>
    /// 角色权限关联表
    /// </summary>
    public class RolePermission : Entity, IHasCreationTime
    {
        public Guid RoleId { get; set; }

        public Guid PermissionId { get; set; }

        public DateTime CreationTime { get; set; }

        public override object?[] GetKeys()
        {
            return new object[] { RoleId, PermissionId };
        }
    }
}
