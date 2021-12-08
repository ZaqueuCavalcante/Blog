using Blog.Exceptions;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Blog.Database;

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
                .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = null);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Blog", Version = "v1" });
            });

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddDbContext<BlogContext>(options => { options
                .UseNpgsql(Configuration.GetConnectionString("Connection"))
                .UseSnakeCaseNamingConvention()
                .EnableDetailedErrors();
            });

            services.AddCors();

            // services.AddIdentity<IdentityUser, IdentityRole>()
            //     .AddEntityFrameworkStores<BergleContext>();
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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog v1"));

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

            app.UseAuthorization();

            app.UseMiddleware<DomainExceptionMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
