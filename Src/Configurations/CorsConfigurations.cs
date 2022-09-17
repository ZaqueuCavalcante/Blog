using Blog.Settings;

namespace Blog.Configurations;

public static class CorsConfigurations
{
    public static void AddCorsConfigurations(this IServiceCollection services)
    {
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
    }

    public static void UseCorsThings(this IApplicationBuilder app)
    {
        if (Env.IsDevelopment())
        {
            app.UseCors("Development");
        }
        if (Env.IsProduction())
        {
            app.UseCors("Production");
            app.UseHsts();
        }
    }
}
