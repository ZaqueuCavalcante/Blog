using System.Reflection;
using Microsoft.OpenApi.Models;

namespace Bolg.Services
{
    public static class SwaggerConfigurations
    {
        public static IServiceCollection AddSwaggerConfigurations(
            this IServiceCollection services,
            IConfiguration configuration
        ) {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Blog",
                    Version = "1.0",
                    Description = "A API to a simple blog engine."
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            });

            return services;
        }
    }
}
