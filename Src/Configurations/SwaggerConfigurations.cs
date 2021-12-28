using System.Reflection;
using Blog.Filters;
using Microsoft.OpenApi.Models;

namespace Blog.Configurations
{
    public static class SwaggerConfigurations
    {
        public static IServiceCollection AddSwaggerConfigurations(
            this IServiceCollection services,
            IConfiguration configuration
        ) {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Blog",
                    Version = "1.0",
                    Description = "A API to a simple blog engine.",
                    Contact = new OpenApiContact() { Name = "Zaqueu", Email = "zaqueudovale@gmail.com" },
                    TermsOfService = new Uri("https://docs.github.com"),
                    License = new OpenApiLicense() { Name = "License", Url = new Uri("https://opensource.org/licenses/MIT") }
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });

                options.ParameterFilter<SwaggerParametersFilter>();

                options.DescribeAllParametersInCamelCase();

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            })
            .AddSwaggerGenNewtonsoftSupport();

            return services;
        }
    }
}
