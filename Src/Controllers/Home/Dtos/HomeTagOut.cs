using Blog.Domain;

namespace Blog.Controllers.Home;

public class HomeTagOut
{
    public int Id { get; set; }
    public string Link { get; set; }
    public string Name { get; set; }

    public HomeTagOut() {}

    public HomeTagOut(Tag tag, string url)
    {
        Id = tag.Id;
        Link = url + "tags/" + tag.Id;
        Name = tag.Name;
    }
}
