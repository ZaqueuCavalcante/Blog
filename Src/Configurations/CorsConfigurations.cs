namespace Blog.Configurations;

public static class CorsConfigurations
{
    public static void AddCorsConfigurations(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );
        });
    }

    public static void UseCorsThings(this IApplicationBuilder app)
    {
        app.UseCors("CorsPolicy");
    }
}
