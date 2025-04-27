namespace TKP.Server.Domain.Permissions
{
    public static class FeaturePermissions
    {
        public static class Post
        {
            public static readonly Permission All = new Permission("Permission.Post", "All permission of Post");
            public static readonly Permission Edit = new Permission("Permission.Post.Edit", "Edit Post");
            public static readonly Permission Create = new Permission("Permission.Post.Create", "Create Post");
            public static readonly Permission View = new Permission("Permission.Post.View", "View Post");
            public static readonly Permission Delete = new Permission("Permission.Post.Delete", "Delete Post");
        }

        public static class User
        {
            public static readonly Permission All = new Permission("Permission.Post", "All permission of Post");
            public static readonly Permission View = new Permission("Permission.User.View", "View User");
            public static readonly Permission Edit = new Permission("Permission.User.Edit", "Edit User");
        }
    }
}
