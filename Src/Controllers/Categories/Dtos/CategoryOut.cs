using Blog.Domain;
using Blog.Extensions;

namespace Blog.Controllers.Categories;

public class CategoryOut
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string CreatedAt { get; set; }

    public CategoryOut() {}

    public CategoryOut(Category category)
    {
        Id = category.Id;
        Name = category.Name;
        Description = category.Description;
        CreatedAt = category.CreatedAt.Format();
    }
}
