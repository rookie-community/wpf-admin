using System;
using Volo.Abp.Application.Dtos;

namespace Admin.OrganizationUnits
{
    /// <summary>
    /// 获取组织单元列表输入
    /// </summary>
    public class GetOrganizationUnitsInput : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 过滤条件（机构名称）
        /// </summary>
        public string? Filter { get; set; }

        /// <summary>
        /// 父级ID（null表示获取根节点）
        /// </summary>
        public Guid? ParentId { get; set; }
    }
}
