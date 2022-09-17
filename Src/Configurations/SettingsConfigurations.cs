using Blog.Settings;

namespace Blog.Configurations;

public static class SettingsConfigurations
{
    public static void AddSettingsConfigurations(this IServiceCollection services)
    {
        services.AddSingleton<DatabaseSettings>();
        services.AddSingleton<JwtSettings>();
    }
}
