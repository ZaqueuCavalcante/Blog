using Blog.Database;
using Microsoft.EntityFrameworkCore;

namespace Blog.Services
{
    public static class EfCoreConfigurations
    {
        public static IServiceCollection AddEfCoreConfigurations(
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

            return services;
        }
    }
}
