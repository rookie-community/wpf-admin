using Volo.Abp.Application.Dtos;

namespace Admin.Roles
{
    public class GetRoleListDto : PagedAndSortedResultRequestDto
    {
        public string? Name { get; set; }
    }
}
