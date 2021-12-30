using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

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
                    .AddJsonFile("appsettings.Test.json")
                    .Build();

                config.AddConfiguration(Configuration);
            });
        }
    }
}
