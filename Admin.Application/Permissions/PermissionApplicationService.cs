using Admin.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using Volo.Abp.Authorization.Permissions;

namespace Admin.Permissions
{
    public class PermissionApplicationService : AdminAppService, IPermissionApplicationService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IPermissionDefinitionManager _permissionDefinitionManager;
        private readonly IStringLocalizerFactory _stringLocalizerFactory;

        public PermissionApplicationService(
            IPermissionRepository permissionRepository,
            IPermissionDefinitionManager permissionDefinitionManager,
            IStringLocalizerFactory stringLocalizerFactory
            )
        {
            _permissionRepository = permissionRepository;
            _permissionDefinitionManager = permissionDefinitionManager;
            _stringLocalizerFactory = stringLocalizerFactory;
        }

        [Authorize]
        public async Task<IReadOnlyList<PermissionDefinitionDto>> GetPermissionDefinitions()
        {
            var permissionDefinitions = await _permissionDefinitionManager.GetPermissionsAsync();
            //var user = CurrentUser;
            var permissions = permissionDefinitions.Select(x => new PermissionDefinitionDto
            {
                Name = x.Name,
                DisplayName = x.DisplayName.Localize(_stringLocalizerFactory),
            }).ToList();
            return await Task.FromResult(permissions);
        }
    }
}
