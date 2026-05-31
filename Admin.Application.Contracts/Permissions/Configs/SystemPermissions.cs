namespace Admin.Permissions.Configs;

public static class SystemPermissions
{
    public const string GroupName = "System";

    public static class Users
    {
        public const string Default = GroupName + ".Users";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";

        public const string Info = Default + ".Info";
        public const string EditInfo = Default + ".EditInfo";
        public const string ResetPassword = Default + ".ResetPassword";
    }

    // *** ADDED a NEW NESTED CLASS ***
    public static class Roles
    {
        public const string Default = GroupName + ".Roles";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
}
