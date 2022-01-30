using Blog.Database;
using Microsoft.EntityFrameworkCore;

namespace Blog.Configurations;

public static class EfCoreConfigurations
{
    public static void AddEfCoreConfigurations(
        this IServiceCollection services,
        IConfiguration configuration
    ) {
        services.AddDbContext<BlogContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Connection"));
            options.UseSnakeCaseNamingConvention();
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
        });
    }
}
