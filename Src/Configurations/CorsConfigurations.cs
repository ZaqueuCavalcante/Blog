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
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (environment == "Development")
        {
            app.UseCors("Development");
        }
        else
        {
            app.UseCors("Production");
            app.UseHsts();
        }
    }
}
