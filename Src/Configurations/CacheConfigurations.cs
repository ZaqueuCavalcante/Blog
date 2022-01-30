namespace Blog.Configurations;

public static class CacheConfigurations
{
    public static void AddCacheConfigurations(this IServiceCollection services)
    {
        services.AddResponseCaching();
    }
}
