using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace Admin.Identity
{
    /// <summary>
    /// 用户角色关联表
    /// </summary>
    public class UserRole : Entity, IHasCreationTime
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }

        public DateTime CreationTime { get; set; }

        public override object[] GetKeys()
        {
            return new object[] { UserId, RoleId };
        }
    }
}
