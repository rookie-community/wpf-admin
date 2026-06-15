using Admin.Commons;
using Admin.Desktop.View;
using Admin.Desktop.View.Permissions;
using Admin.Desktop.View.Reports;
using Admin.Desktop.View.SettingManagement.EmailSettings;
using Admin.Desktop.View.Tenants;
using Admin.Desktop.View.Users;
using Volo.Abp.Identity;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace Admin.Desktop.Tools
{
    public static class NavProvider
    {
        public static IReadOnlyList<NavDto> GetNavConfigs()
        {
            var menus = new List<NavDto>
            {
                GetHomeGroup(),
                GetReportGroup(),
                GetSystemGroup(),
                GetAbourGroup()
            };
            return menus;
        }

        private static NavDto GetHomeGroup()
        {
            return new NavDto
            {
                Id = Guid.NewGuid(),
                Icon = "\xf015;",
                Name = "控制台",
                Type = NavType.UserControl,
                Content = typeof(ConsoleView).FullName,
            };
        }

        private static NavDto GetSystemGroup()
        {
            return new NavDto
            {
                Id = Guid.NewGuid(),
                Icon = "\xf013",
                Name = "系统设置",
                Type = NavType.Group,
                PermissionName = IdentityPermissions.GroupName,
                Items = new List<NavDto>
                {
                    new NavDto
                    {
                        Id = Guid.NewGuid(),
                        Icon = "\xf2b9",
                        Name = "角色管理",
                        Type = NavType.UserControl,
                        PermissionName = IdentityPermissions.Roles.Default,
                        Content = typeof(RoleView).FullName,
                    },
                    new NavDto
                    {
                        Id = Guid.NewGuid(),
                        Icon = "\xf007",
                        Name = "用户管理",
                        Type = NavType.UserControl,
                        PermissionName = IdentityPermissions.Users.Default,
                        Content = typeof(UserView).FullName,
                    },
                    new NavDto
                    {
                        Id = Guid.NewGuid(),
                        Icon = "\xe4da",
                        Name = "租户管理",
                        Type = NavType.UserControl,
                        PermissionName = TenantManagementPermissions.Tenants.Default,
                        Content = typeof(TenantView).FullName,
                    },
                    new NavDto
                    {
                        Id = Guid.NewGuid(),
                        Icon = "\xf0e0",
                        Name = "邮件服务",
                        Type = NavType.UserControl,
                        PermissionName = SettingManagementPermissions.Emailing,
                        Content = typeof(EmailSettingView).FullName,
                    },
                }
            };
        }

        private static NavDto GetReportGroup()
        {
            return new NavDto
            {
                Id = Guid.NewGuid(),
                Icon = "\xf201",
                Name = "报表设计",
                Type = NavType.UserControl,
                Content = typeof(ReportDesign).FullName,
            };
        }

        private static NavDto GetAbourGroup()
        {
            return new NavDto
            {
                Id = Guid.NewGuid(),
                Icon = "\xf05a",
                Name = "关于",
                Type = NavType.Url,
                Content = "https://gitee.com/tzm2270969436",
            };
        }
    }
}
