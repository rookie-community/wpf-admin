using Volo.Abp.Application.Dtos;

namespace Admin.Users
{
    public class UserDto : EntityDto<Guid>
    {
        public string Account { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public bool IsActive { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }
    }
}
