using Volo.Abp.Application.Dtos;

namespace Admin.Users
{
    public class UpdateCurrentPasswordDto : EntityDto<Guid>
    {
        public string OldPassword { get; set; } = null!;

        public string NewPassword { get; set; } = null!;
    }
}
