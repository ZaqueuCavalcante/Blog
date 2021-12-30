namespace Blog.Configurations
{
    public static class CacheConfigurations
    {
        public static IServiceCollection AddCacheConfigurations(this IServiceCollection services)
        {
            services.AddResponseCaching();

            return services;
        }
    }
}
