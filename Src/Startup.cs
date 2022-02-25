using Blog.Configurations;

namespace Blog;

public class Startup
{
    public Startup() {}

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSettingsConfigurations();

        services.AddControllersConfigurations();

        services.AddSwaggerConfigurations();

        services.AddRoutingConfigurations();

        services.AddEfCoreConfigurations();

        services.AddCorsConfigurations();

        services.AddIdentityConfigurations();

        services.AddAuthenticationConfigurations();

        services.AddAuthorizationConfigurations();

        services.AddCacheConfigurations();

        services.AddHealthChecks();
    }

    public void Configure(
        IApplicationBuilder app,
        IWebHostEnvironment env
    ) {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog 1.0"));

            app.UseCors("Development");
        }
        else
        {
            app.UseCors("Production");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseResponseCaching();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseDomainExceptions();

        app.UseEndpoints(builder =>
        {
            builder.MapControllers();
            builder.MapHealthChecks("/healthz");
        });
    }
}
