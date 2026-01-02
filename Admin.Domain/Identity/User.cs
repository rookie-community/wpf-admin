using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Admin.Identity
{
    /// <summary>
    /// 用户表
    /// </summary>
    public class User : AggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// 账户
        /// </summary>
        public string Account { get; set; } = null!;

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; } = null!;

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = null!;

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; } = null!;

        public Guid? TenantId { get; set; }
    }
}
