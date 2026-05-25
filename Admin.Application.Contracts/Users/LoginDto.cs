using Volo.Abp.Application.Dtos;

namespace Admin.Users
{
    public class LoginDto : EntityDto
    {
        public string? Tenant { get; set; }

        public string Account { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
