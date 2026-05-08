namespace AuthenticationSystem.Application.Constants;

public static class AppPermissions
{
    public static class Users
    {
        public const string View   = "Users.View";
        public const string Create = "Users.Create";
        public const string Edit   = "Users.Edit";
        public const string Delete = "Users.Delete";
    }

    public static class Roles
    {
        public const string View   = "Roles.View";
        public const string Create = "Roles.Create";
        public const string Edit   = "Roles.Edit";
        public const string Delete = "Roles.Delete";
    }

    public static class Modules
    {
        public const string View   = "Modules.View";
        public const string Create = "Modules.Create";
        public const string Edit   = "Modules.Edit";
        public const string Delete = "Modules.Delete";
    }

    public static class Perms
    {
        public const string View   = "Permissions.View";
        public const string Create = "Permissions.Create";
        public const string Edit   = "Permissions.Edit";
        public const string Delete = "Permissions.Delete";
    }

    public static IEnumerable<string> GetAll() =>
    [
        Users.View,   Users.Create,   Users.Edit,   Users.Delete,
        Roles.View,   Roles.Create,   Roles.Edit,   Roles.Delete,
        Modules.View, Modules.Create, Modules.Edit, Modules.Delete,
        Perms.View,   Perms.Create,   Perms.Edit,   Perms.Delete
    ];
}
