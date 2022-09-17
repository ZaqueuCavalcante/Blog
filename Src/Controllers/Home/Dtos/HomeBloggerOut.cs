using Blog.Domain;

namespace Blog.Controllers.Home;

public class HomeBloggerOut
{
    public int Id { get; set; }
    public string Link { get; set; }
    public string Name { get; set; }
    public string Resume { get; set; }
    public List<object> Networks { get; set; }

    public HomeBloggerOut() {}

    public HomeBloggerOut(Blogger blogger, string url)
    {
        Id = blogger.Id;
        Link = url + "bloggers/" + blogger.Id;
        Name = blogger.Name;
        Resume = blogger.Resume;
        Networks = blogger.Networks?.Select(n => (object) new { Name = n.Name, Uri = n.Uri }).ToList();
    }
}
