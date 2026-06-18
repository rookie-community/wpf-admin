using System;
using System.ComponentModel.DataAnnotations;

namespace Admin.OrganizationUnits
{
    /// <summary>
    /// 更新组织单元输入
    /// </summary>
    public class UpdateOrganizationUnitInput
    {
        /// <summary>
        /// 机构ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 机构名称
        /// </summary>
        [Required]
        [StringLength(128)]
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// 排序号
        /// </summary>
        public int SortOrder { get; set; }
    }
}
