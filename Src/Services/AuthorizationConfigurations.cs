namespace Blog.Services
{
    public static class AuthorizationConfigurations
    {
        public const string CommentPinPolicy = "CommentPinPolicy";
        public const string CommentLikePolicy = "CommentLikePolicy";

        public static IServiceCollection AddAuthorizationConfigurations(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(CommentPinPolicy, policy =>
                {
                    policy.RequireRole("Blogger");
                    policy.RequireClaim("pinner", "true");
                });

                options.AddPolicy(CommentLikePolicy, policy =>
                {
                    policy.RequireRole("Reader", "Blogger");
                    policy.RequireClaim("liker", "true");
                });
            });

            return services;
        }
    }
}
