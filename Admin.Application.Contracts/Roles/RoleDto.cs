using Volo.Abp.Application.Dtos;

namespace Admin.Roles
{
    public class RoleDto : EntityDto<Guid>
    {
        public string Name { get; set; } = null!;

        public bool IsDefault { get; set; }
    }
}
