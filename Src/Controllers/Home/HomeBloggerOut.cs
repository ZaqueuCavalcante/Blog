using Blog.Domain;

namespace Blog.Controllers.Home
{
    public class HomeBloggerOut
    {
        public int Id { get; set; }
        public string Link { get; set; }
        public string Name { get; set; }
        public string Resume { get; set; }
        public List<object> Networks { get; set; }

        public static HomeBloggerOut New(Blogger blogger, List<Network> networks, string url)
        {
            return new HomeBloggerOut
            {
                Id = blogger.Id,
                Link = url + "bloggers/" + blogger.Id,
                Name = blogger.Name,
                Resume = blogger.Resume,
                Networks = networks?.Select(n => (object) new { Name = n.Name, Uri = n.Uri }).ToList()
            };
        }
    }
}
