namespace Blog.Configurations
{
    public static class RoutingConfigurations
    {
        public static void AddRoutingConfigurations(this IServiceCollection services)
        {
            services.AddRouting(options => options.LowercaseUrls = true);
        }
    }
}
