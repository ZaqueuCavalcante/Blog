using Blog.Services.Bloggers;
using Blog.Services.Categories;

namespace Blog.Configurations;

public static class BlogServicesConfigurations
{
    public static void AddBlogServicesConfigurations(this IServiceCollection services)
    {
        services.AddScoped<ICategoriesService, CategoriesService>();
        services.AddScoped<IBloggersService, BloggersService>();
    }
}
