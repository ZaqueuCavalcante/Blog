using Blog.Services.Bloggers;
using Blog.Services.Categories;
using Blog.Services.Posts;

namespace Blog.Configurations;

public static class BlogServicesConfigurations
{
    public static void AddBlogServicesConfigurations(this IServiceCollection services)
    {
        services.AddScoped<ICategoriesService, CategoriesService>();
        services.AddScoped<IBloggersService, BloggersService>();
        services.AddScoped<IPostsService, PostsService>();
    }
}
