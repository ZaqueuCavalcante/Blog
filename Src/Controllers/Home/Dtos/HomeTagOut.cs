using Blog.Domain;

namespace Blog.Controllers.Home
{
    public class HomeTagOut
    {
        public int Id { get; set; }
        public string Link { get; set; }
        public string Name { get; set; }

        public static HomeTagOut New(Tag tag, string url)
        {
            return new HomeTagOut
            {
                Id = tag.Id,
                Link = url + "tags/" + tag.Id,
                Name = tag.Name
            };
        }
    }
}
