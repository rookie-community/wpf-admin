using Volo.Abp.Application.Dtos;

namespace Admin.Users
{
    public class UserDto : EntityDto<Guid>
    {
        public string UserName { get; set; } = null!;

        public string? Email { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }
    }
}
