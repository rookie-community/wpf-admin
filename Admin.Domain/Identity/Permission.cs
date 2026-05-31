using Volo.Abp.Domain.Entities;

namespace Admin.Identity
{
    /// <summary>
    /// 权限表/菜单表
    /// </summary>
    public class Permission : AggregateRoot<Guid>
    {
        public string GroupName { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string ParentName { get; set; } = null!;

        public string DisplayName { get; set; } = null!;

        public bool IsEnable { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        //public PermissionType PermissionType { get; set; }

        ///// <summary>
        ///// 父节点
        ///// </summary>
        //public Guid? ParentId { get; set; }

        ///// <summary>
        ///// 页面名称
        ///// </summary>
        //public string ViewName { get; set; } = null!;

        //public string Description { get; set; } = null!;

        //public List<Permission> Children { get; set; } = new List<Permission>();
    }
}
