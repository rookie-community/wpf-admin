using Volo.Abp.Application.Dtos;

namespace Admin.Users
{
    public class UpdateCurrentUserDto : EntityDto<Guid>
    {
        public string UserName { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}
