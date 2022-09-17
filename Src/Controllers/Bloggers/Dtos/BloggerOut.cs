using Blog.Domain;
using Blog.Extensions;

namespace Blog.Controllers.Bloggers;

public class BloggerOut
{
    /// <example>
    /// 420
    /// </example>
    public int Id { get; set; }

    /// <example>
    /// Bob Marley
    /// </example>
    public string Name { get; set; }

    /// <example>
    /// I write about things.
    /// </example>
    public string Resume { get; set; }

    public List<object> Networks { get; set; }
    public List<object> Posts { get; set; }

    public BloggerOut() {}

    public BloggerOut(Blogger blogger)
    {
        Id = blogger.Id;
        Name = blogger.Name;
        Resume = blogger.Resume;
        Networks = blogger.Networks?.Select(n => (object) new { Name = n.Name, Uri = n.Uri }).ToList();
        Posts = blogger.Posts?.Select(p => (object) new
        {
            Id = p.Id,
            Title = p.Title,
            CreatedAt = p.CreatedAt.Format(),
            Resume = p.Resume,
        }).ToList();
    }
}
