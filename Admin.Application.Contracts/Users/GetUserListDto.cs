using Volo.Abp.Application.Dtos;

namespace Admin.Users
{
    public class GetUserListDto : PagedAndSortedResultRequestDto
    {
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}
