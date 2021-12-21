namespace Blog.Services
{
    public static class AuthorizationConfigurations
    {
        public static IServiceCollection AddAuthorizationConfigurations(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("CommentPinPolicy", policy =>
                {
                    policy.RequireRole("Blogger");
                    policy.RequireClaim("pinner", "true");
                });
            });

            return services;
        }
    }
}
