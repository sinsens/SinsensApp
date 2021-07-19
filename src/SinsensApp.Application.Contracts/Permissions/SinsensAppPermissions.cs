namespace SinsensApp.Permissions
{
    public static class SinsensAppPermissions
    {
        public const string GroupName = "SinsensApp";

        //Add your own permission names. Example:
        //public const string MyPermission1 = GroupName + ".MyPermission1";

        public class Transaction
        {
            public const string Default = GroupName + ".Transaction";
            public const string Update = Default + ".Update";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }

        public class Account
        {
            public const string Default = GroupName + ".Account";
            public const string Update = Default + ".Update";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }

        public class Tag
        {
            public const string Default = GroupName + ".Tag";
            public const string Update = Default + ".Update";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }

        public class Category
        {
            public const string Default = GroupName + ".Category";
            public const string Update = Default + ".Update";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }

        public class TransactionAttachment
        {
            public const string Default = GroupName + ".TransactionAttachment";
            public const string Update = Default + ".Update";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }
    }
}
