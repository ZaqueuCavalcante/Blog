using Blog.Database;
using Blog.Domain;
using Blog.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Blog.Services.Categories;

public class CategoriesService : ICategoriesService
{
    private readonly BlogContext _context;

    public CategoriesService(BlogContext context) => _context = context;

    public async Task<Category> CreateCategory(string name, string description)
    {
        var category = new Category(name, description);

        var categoryAlreadyExists = await _context.Categories.AnyAsync(
            c => c.Name.ToLower() == name.Trim().ToLower());

        if (categoryAlreadyExists)
            throw new DomainException("Category already exists.");

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return category;
    }

    public async Task<Category> GetCategory(int id)
    {
        var category = await _context.Categories.AsNoTracking()
            .Include(c => c.Posts.OrderByDescending(p => p.CreatedAt))
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category is null)
            throw new DomainException("Category not found.", 404);

        return category;
    }

    public async Task<List<Category>> GetCategories()
    {
        var categories = await _context.Categories.AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync();

        return categories;
    }
}
