using Volo.Abp.Application.Dtos;

namespace Admin.Users
{
    public class CurrentUserDto : EntityDto<Guid>
    {
        public string TenantName { get; set; } = null!;

        public string Account { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public bool IsActive { get; set; }

        public string? PhoneNumber { get; set; }

        public string Email { get; set; } = null!;
    }
}
