using Blog.Database;
using Blog.Settings;
using Microsoft.EntityFrameworkCore;

namespace Blog.Configurations;

public static class EfCoreConfigurations
{
    public static void AddEfCoreConfigurations(this IServiceCollection services)
    {
        var databaseSettings = services.BuildServiceProvider().GetService<DatabaseSettings>();

        services.AddDbContext<BlogContext>(options =>
        {
            options.UseNpgsql(databaseSettings.ConnectionString);
            options.UseSnakeCaseNamingConvention();
            if (Env.IsDevelopment())
            {
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            }
        });
    }
}
