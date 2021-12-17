using Blog.Domain;
using Blog.Extensions;

namespace Blog.Controllers.Tags
{
    public class TagOut
    {
        public string Name { get; set; }

        public string CreatedAt { get; set; }

        public List<object> Posts { get; set; }

        public TagOut(Tag tag)
        {
            Name = tag.Name;
            CreatedAt = tag.CreatedAt.Format();
            Posts = tag.Posts?
                .Select(p => (object) new { Date = p.CreatedAt.Format(), Title = p.Title })
                    .ToList();
        }
    }
}
