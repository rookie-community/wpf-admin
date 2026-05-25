using Volo.Abp.Application.Dtos;

namespace Admin.Users
{
    public class UserDto : EntityDto<Guid>
    {
        public string Account { get; set; } = null!;

        public string UserName { get; set; } = null!;
    }
}
