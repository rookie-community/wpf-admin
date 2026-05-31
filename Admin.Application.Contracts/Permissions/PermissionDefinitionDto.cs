using Volo.Abp.Application.Dtos;
using Volo.Abp.MultiTenancy;

namespace Admin.Permissions
{
    public class PermissionDefinitionDto : EntityDto
    {
        public string Name { get; set; } = null!;

        public string DisplayName { get; set; } = null!;

        public PermissionDefinitionDto? Parent { get; set; }

        public MultiTenancySides MultiTenancySide { get; set; }

        public bool IsEnabled { get; set; }

        public IReadOnlyList<PermissionDefinitionDto> Children { get; set; } = new List<PermissionDefinitionDto>();
    }
}
