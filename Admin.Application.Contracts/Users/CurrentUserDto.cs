using Volo.Abp.Application.Dtos;

namespace Admin.Users
{
    public class CurrentUserDto : EntityDto<Guid>
    {
        public string Email { get; set; } = null!;

        public string UserName { get; set; } = null!;
        public Guid? TenantId { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
