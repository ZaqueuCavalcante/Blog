using Blog.Domain;

namespace Blog.Services.Categories;

public interface ICategoriesService
{
    public Task<Category> CreateCategory(string name, string description);

    public Task<Category> GetCategory(int id);

    public Task<List<Category>> GetCategories();
}
