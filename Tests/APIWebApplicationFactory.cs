using Blog.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Tests
{
    public class APIWebApplicationFactory : WebApplicationFactory<Startup>
    {
        public IConfiguration Configuration { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(config =>
            {
                Configuration = new ConfigurationBuilder()
                    .SetBasePath("C:\\Users\\Zaqueu\\blog\\Tests\\")  // TODO: refactor this
                    .AddJsonFile("appsettings.Test.json").Build();

                config.AddConfiguration(Configuration);
            });

            builder.ConfigureTestServices(services =>
            {
                services.AddDbContext<BlogContext>(options =>
                {
                    options.UseNpgsql(Configuration.GetConnectionString("Connection"));
                    options.UseSnakeCaseNamingConvention();
                    options.EnableDetailedErrors();
                    options.EnableSensitiveDataLogging();
                });
            });
        }
    }
}
