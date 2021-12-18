using Blog.Domain;

namespace Blog.Controllers.Bloggers
{
    public class BloggerOut
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Resume { get; set; }
        public List<object> Networks { get; set; }

        public static BloggerOut New(Blogger blogger, List<Network>? networks = null)
        {
            return new BloggerOut
            {
                Id = blogger.Id,
                Name = blogger.Name,
                Resume = blogger.Resume,
                Networks = networks?.Select(n => (object) new { Name = n.Name, Uri = n.Uri }).ToList()
            };
        }
    }
}
