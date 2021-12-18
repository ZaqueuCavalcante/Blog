using Blog.Domain;
using Blog.Extensions;

namespace Blog.Controllers.Categories
{
    public class CategoryOut
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedAt { get; set; }

        public List<object> Posts { get; set; }

        public static CategoryOut New(Category category)
        {
            return new CategoryOut
            {
                Name = category.Name,
                Description = category.Description,
                CreatedAt = category.CreatedAt.Format(),
                Posts = category.Posts?
                    .Select(p => (object) new { Date = p.CreatedAt.Format(), Title = p.Title })
                    .ToList()
            };
        }
    }
}
