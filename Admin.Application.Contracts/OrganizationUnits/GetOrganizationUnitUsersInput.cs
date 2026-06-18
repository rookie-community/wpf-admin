using System;
using Volo.Abp.Application.Dtos;

namespace Admin.OrganizationUnits
{
    /// <summary>
    /// 获取组织单元成员列表输入
    /// </summary>
    public class GetOrganizationUnitUsersInput : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 组织单元ID
        /// </summary>
        public Guid OrganizationUnitId { get; set; }

        /// <summary>
        /// 过滤条件（用户名）
        /// </summary>
        public string? Filter { get; set; }
    }
}
