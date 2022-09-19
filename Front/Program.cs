using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Front;
using MudBlazor.Services;

namespace Blog.Front;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddHttpClient("BlogApi", client =>
        {
            client.BaseAddress = new Uri("https://localhost:5000");
        });
        builder.Services.AddScoped(sp => sp.GetService<IHttpClientFactory>()!.CreateClient("BlogApi"));

        builder.Services.AddMudServices();

        await builder.Build().RunAsync();
    }
}
