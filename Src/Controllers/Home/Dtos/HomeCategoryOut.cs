using Blog.Domain;

namespace Blog.Controllers.Home;

public class HomeCategoryOut
{
    public int Id { get; set; }
    public string Link { get; set; }
    public string Name { get; set; }
    public int Posts { get; set; }

    public HomeCategoryOut() {}

    public HomeCategoryOut(Category category, string url)
    {
        Id = category.Id;
        Link = url + "categories/" + category.Id;
        Name = category.Name;
        Posts = category.Posts.Count;
    }
}
