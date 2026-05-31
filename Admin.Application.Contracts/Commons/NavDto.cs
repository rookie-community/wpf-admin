using Volo.Abp.Application.Dtos;

namespace Admin.Commons
{
    public class NavDto : EntityDto<Guid>
    {
        public string? Icon { get; set; }

        public string Name { get; set; } = null!;

        public NavType Type { get; set; }

        public string? Content { get; set; }

        public string? PermissionName { get; set; }

        public IReadOnlyList<NavDto> Items { get; set; } = new List<NavDto>();
    }
}
