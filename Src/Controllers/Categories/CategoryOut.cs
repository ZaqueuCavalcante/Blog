using Blog.Domain;

namespace Blog.Controllers.Categories
{
    public class CategoryOut
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedAt { get; set; }

        public List<object> Posts { get; set; }

        public CategoryOut(Category category)
        {
            Name = category.Name;
            Description = category.Description;
            CreatedAt = category.CreatedAt.ToString("dd/MM/yyyy HH:mm");
            Posts = category.Posts?
                .Select(p => (object) new { Date = p.CreatedAt.ToString("dd/MM/yyyy HH:mm"), Title = p.Title })
                .ToList();
        }
    }
}
