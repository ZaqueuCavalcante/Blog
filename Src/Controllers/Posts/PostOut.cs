using Blog.Domain;
using Blog.Extensions;

namespace Blog.Controllers.Posts
{
    public class PostOut
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Resume { get; set; }
        public string Body { get; set; }
        public byte Rating { get; set; }
        public string CreatedAt { get; set; }
        public List<string> Authors { get; set; }
        public List<CommentOut> Comments { get; set; }
        public List<string> Tags { get; set; }

        public static PostOut New(Post post)
        {
            return new PostOut
            {
                Id = post.Id,
                Title = post.Title,
                Resume = post.Resume,
                Body = post.Body,
                Rating = post.GetRating(),
                CreatedAt = post.CreatedAt.Format(),
                Authors = post.Authors?.Select(b => b.Name).ToList(),
                Comments = post.Comments?.Select(c => new CommentOut(c)).ToList(),
                Tags = post.Tags?.Select(c => c.Name).ToList()
            };
        }
    }
}
