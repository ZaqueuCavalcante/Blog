using Blog.Domain;
using Blog.Extensions;

namespace Blog.Controllers.Bloggers;

public class BloggerOut
{
    /// <example>42</example>
    public int Id { get; set; }

    /// <example>xyz/bloggers/42</example>
    public string Link { get; set; }

    /// <example>Bob</example>
    public string Name { get; set; }

    /// <example>I write about things.</example>
    public string Resume { get; set; }

    public List<object> Networks { get; set; }
    public List<object> Posts { get; set; }

    public static BloggerOut New(Blogger blogger, List<Network>? networks = null, string? root = null)
    {
        return new BloggerOut
        {
            Id = blogger.Id,
            Link = root + "bloggers/" + blogger.Id,
            Name = blogger.Name,
            Resume = blogger.Resume,
            Networks = networks?.Select(n => (object) new { Name = n.Name, Uri = n.Uri }).ToList(),
            Posts = blogger.Posts?.Select(p => (object) new
            {
                Id = p.Id,
                Link = root + $"posts/{p.Id}",
                Title = p.Title,
                CreatedAt = p.CreatedAt.Format(),
                Resume = p.Resume
            }).ToList()
        };
    }
}
