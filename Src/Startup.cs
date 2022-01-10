using Blog.Extensions;
using Blog.Configurations;
using Blog.Services;

namespace Blog
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersConfigurations();

            services.AddSwaggerConfigurations(Configuration);

            services.AddRoutingConfigurations();

            services.AddEfCoreConfigurations(Configuration);

            services.AddCorsConfigurations();

            services.AddIdentityConfigurations();

            services.AddAuthenticationConfigurations(Configuration);

            services.AddAuthorizationConfigurations();

            services.AddCacheConfigurations();

            services.AddHealthChecks();

            services.AddScoped<IEmailSender, EmailSender>();
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
}
