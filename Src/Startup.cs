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

            services.AddCors();

            services.AddIdentityConfigurations(Configuration);
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

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                DbSeeder.Seed(context);
            }

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()); 

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
