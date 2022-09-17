using Blog.Domain;
using Blog.Extensions;

namespace Blog.Controllers.Tags;

public class TagOut
{
    public int Id { get; set; }
    public string Link { get; set; }
    public string Name { get; set; }
    public string CreatedAt { get; set; }
    public List<object> Posts { get; set; }

    public TagOut() {}

    public TagOut(Tag tag, string url)
    {
        Id = tag.Id;
        Link = url + "tags/" + tag.Id;
        Name = tag.Name;
        CreatedAt = tag.CreatedAt.Format();
        Posts = tag.Posts?
            .Select(p => (object) new {
                Id = p.Id,
                Link = url + "posts/" + p.Id,
                CreatedAt = p.CreatedAt.Format(),
                Title = p.Title
            }).ToList();
    }
}
