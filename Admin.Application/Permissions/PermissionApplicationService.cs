using Admin.Identity;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Authorization.Permissions;

namespace Admin.Permissions
{
    public class PermissionApplicationService : AdminAppService, IPermissionApplicationService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IPermissionDefinitionManager permissionDefinitionManager;

        public PermissionApplicationService(IPermissionRepository permissionRepository, IPermissionDefinitionManager permissionDefinitionManager)
        {
            _permissionRepository = permissionRepository;
            this.permissionDefinitionManager = permissionDefinitionManager;
        }

        [Authorize]
        public async Task<IReadOnlyList<PermissionDefinitionDto>> GetPermissionDefinitions()
        {
            var permissionDefinitions = await permissionDefinitionManager.GetPermissionsAsync();
            //var user = CurrentUser;
            var permissions = new List<PermissionDefinitionDto>();
            return await Task.FromResult(permissions);
        }
    }
}
