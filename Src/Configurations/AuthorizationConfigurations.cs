namespace Blog.Configurations
{
    public static class AuthorizationConfigurations
    {
        public const string AdminRole = "Admin";
        public const string BloggerRole = "Blogger";
        public const string ReaderRole = "Reader";

        public const string CommentPinPolicy = "CommentPinPolicy";
        public const string CommentLikePolicy = "CommentLikePolicy";

        public static void AddAuthorizationConfigurations(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(CommentPinPolicy, policy =>
                {
                    policy.RequireRole(BloggerRole);
                    policy.RequireClaim("pinner", "true");
                });

                options.AddPolicy(CommentLikePolicy, policy =>
                {
                    policy.RequireRole(ReaderRole, BloggerRole);
                    policy.RequireClaim("liker", "true");
                });
            });
        }
    }
}
