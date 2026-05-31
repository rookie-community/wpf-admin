using Admin.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Admin.Permissions.Configs;

public class SystemPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var systemGroup = context.AddGroup(SystemPermissions.GroupName, L("Permission:System"));

        var booksPermission = systemGroup.AddPermission(SystemPermissions.Users.Default, L("Permission:Users"));
        booksPermission.AddChild(SystemPermissions.Users.Create, L("Permission:Users.Create"));
        booksPermission.AddChild(SystemPermissions.Users.Edit, L("Permission:Users.Edit"));
        booksPermission.AddChild(SystemPermissions.Users.Delete, L("Permission:Users.Delete"));

        var authorsPermission = systemGroup.AddPermission(
            SystemPermissions.Roles.Default, L("Permission:Roles"));
        authorsPermission.AddChild(
            SystemPermissions.Roles.Create, L("Permission:Roles.Create"));
        authorsPermission.AddChild(
            SystemPermissions.Roles.Edit, L("Permission:Roles.Edit"));
        authorsPermission.AddChild(
            SystemPermissions.Roles.Delete, L("Permission:Roles.Delete"));

    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AdminResource>(name);
    }
}
