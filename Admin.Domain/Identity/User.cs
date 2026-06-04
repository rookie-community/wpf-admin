using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Admin.Identity
{
    /// <summary>
    /// 用户表
    /// </summary>
    public class User : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

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
        /// 是否激活
        /// </summary>
        public bool IsActive { get; set; }

        public string? PhoneNumber { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; } = null!;
    }
}
