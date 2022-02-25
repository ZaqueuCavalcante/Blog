using Blog.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Converters;
using static Microsoft.AspNetCore.Mvc.ResponseCacheLocation;

namespace Blog.Configurations;

public static class ControllersConfigurations
{
    public const string TwoMinutesCacheProfile = "TwoMinutesCacheProfile";

    public static void AddControllersConfigurations(this IServiceCollection services)
    {
        services.AddControllers(options => 
        {
            options.CacheProfiles.Add(
                TwoMinutesCacheProfile,
                new CacheProfile { Duration = 120, Location = Client }
            );
        })
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.Converters.Add(new StringEnumConverter());
        });
    }

    public static void UseDomainExceptions(this IApplicationBuilder app)
    {
        app.UseMiddleware<DomainExceptionMiddleware>();
    }
}
