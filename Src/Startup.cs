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

    public void Configure(IApplicationBuilder app)
    {
        app.UseCorsThings();

        app.UseHttpsRedirection();

        app.UseResponseCaching();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSwaggerInterface();
        app.UseDomainExceptions();

        app.UseControllersAndEndpoints();
    }
}
