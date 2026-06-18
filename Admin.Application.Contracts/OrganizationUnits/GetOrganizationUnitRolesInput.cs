using System;
using Volo.Abp.Application.Dtos;

namespace Admin.OrganizationUnits
{
    /// <summary>
    /// 获取组织单元角色列表输入
    /// </summary>
    public class GetOrganizationUnitRolesInput : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 组织单元ID
        /// </summary>
        public Guid OrganizationUnitId { get; set; }

        /// <summary>
        /// 过滤条件（角色名称）
        /// </summary>
        public string? Filter { get; set; }
    }
}
