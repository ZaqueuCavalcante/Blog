using Blog.Domain;
using Blog.Extensions;

namespace Blog.Controllers.Home
{
    public class HomePostOut
    {
        public int Id { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        public string CreatedAt { get; set; }
        public string Resume { get; set; }

        public static HomePostOut New(Post post, string url)
        {
            return new HomePostOut
            {
                Id = post.Id,
                Link = url + "posts/" + post.Id,
                Title = post.Title,
                CreatedAt = post.CreatedAt.Format(),
                Resume = post.Resume
            };
        }
    }
}
