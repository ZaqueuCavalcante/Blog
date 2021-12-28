namespace Blog.Configurations
{
    public static class RoutingConfigurations
    {
        public static IServiceCollection AddRoutingConfigurations(this IServiceCollection services)
        {
            services.AddRouting(options => options.LowercaseUrls = true);

            return services;
        }
    }
}
