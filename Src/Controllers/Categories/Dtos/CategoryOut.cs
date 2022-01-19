using Blog.Domain;
using Blog.Extensions;

namespace Blog.Controllers.Categories
{
    public class CategoryOut
    {
        public int Id { get; set; }
        public string Link { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedAt { get; set; }
        public List<object> Posts { get; set; }

        public static CategoryOut New(Category category, string url)
        {
            return new CategoryOut
            {
                Id = category.Id,
                Link = url + "categories/" + category.Id,
                Name = category.Name,
                Description = category.Description,
                CreatedAt = category.CreatedAt.Format(),
                Posts = category.Posts?
                    .Select(p => (object) new {
                        Id = p.Id,
                        Link = url + "posts/" + p.Id,
                        CreatedAt = p.CreatedAt.Format(),
                        Title = p.Title
                    })
                    .ToList()
            };
        }
    }
}
