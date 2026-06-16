using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Admin.Permissions
{
    /// <summary>权限分组树形（外层分组容器，包含分组+分组下整棵权限树）</summary>
    public class PermissionGroupTreeDto : EntityDto
    {
        /// <summary>分组名称</summary>
        public string GroupName { get; set; } = string.Empty;

        /// <summary>分组显示名称</summary>
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>分组下顶层权限树</summary>
        public List<PermissionTreeDto> Permissions { get; set; } = new();
    }
}
