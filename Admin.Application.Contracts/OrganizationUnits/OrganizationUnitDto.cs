using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Admin.OrganizationUnits
{
    public class OrganizationUnitDto : EntityDto<Guid>
    {
        /// <summary>父级Id，顶级机构为null</summary>
        public Guid? ParentId { get; set; }

        /// <summary>机构名称</summary>
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>排序号</summary>
        public int SortOrder { get; set; }

        /// <summary>层级路径（1.2.3）用于树形筛选</summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>子机构集合</summary>
        public List<OrganizationUnitDto> Children { get; set; } = new();

        /// <summary>创建时间</summary>
        public DateTime CreationTime { get; set; }
    }
}
