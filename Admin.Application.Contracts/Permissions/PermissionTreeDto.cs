using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Admin.Permissions
{
    /// <summary>权限树形节点</summary>
    public class PermissionTreeDto : EntityDto
    {
        /// <summary>权限唯一标识（PermissionName）</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>权限显示名称</summary>
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>父权限Name，顶层权限为空</summary>
        public string ParentName { get; set; } = string.Empty;

        /// <summary>所属分组名称</summary>
        public string GroupName { get; set; } = string.Empty;

        public bool IsEditable { get; set; }

        /// <summary>是否已授权</summary>
        public bool IsGranted { get; set; }

        /// <summary>子权限集合（递归树形）</summary>
        public List<PermissionTreeDto> Children { get; set; } = new();
    }
}
