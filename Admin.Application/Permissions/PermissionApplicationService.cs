using Admin.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Admin.Permissions
{
    public class PermissionApplicationService : AdminAppService, IPermissionApplicationService
    {
        private readonly IPermissionRepository _permissionRepository;

        public PermissionApplicationService(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        [Authorize]
        public Task<string> Test()
        {
            var user = CurrentUser;
            return Task.FromResult(user.Name);
        }
    }
}
