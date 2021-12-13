namespace Bolg.Services
{
    public static class CorsConfigurations
    {
        public static IServiceCollection AddCorsConfigurations(
            this IServiceCollection services,
            IConfiguration configuration
        ) {
            services.AddCors(options =>
            {
                options.AddPolicy("Development", builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                );

                options.AddPolicy("Production", builder => builder
                    .WithMethods("GET")
                    .WithOrigins("https://manguebit.com")
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                );
            });

            return services;
        }
    }
}
