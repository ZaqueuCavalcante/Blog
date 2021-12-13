using Blog.Exceptions;
using Microsoft.EntityFrameworkCore;
using Blog.Database;
using Bolg.Services;

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
            services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = null);

            services.AddSwaggerConfigurations(Configuration);

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddDbContext<BlogContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("Connection"));
                options.UseSnakeCaseNamingConvention();
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            });

            services.AddCorsConfigurations(Configuration);

            services.AddIdentityConfigurations(Configuration);

            services.AddJwtConfigurations(Configuration);
        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            BlogContext context
        ) {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog 1.0"));

                app.UseCors("Development");

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                DbSeeder.Seed(context);
            }
            else
            {
                app.UseCors("Production");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<DomainExceptionMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
