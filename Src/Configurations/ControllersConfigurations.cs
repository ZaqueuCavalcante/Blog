using Newtonsoft.Json.Converters;

namespace Blog.Configurations
{
    public static class ControllersConfigurations
    {
        public static IServiceCollection AddControllersConfigurations(this IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            });

            return services;
        }
    }
}
