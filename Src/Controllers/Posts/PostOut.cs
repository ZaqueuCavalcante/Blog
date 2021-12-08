using Blog.Domain;

namespace Blog.Controllers.Posts
{
    public class PostOut
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Resume { get; set; }
        public string Body { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<string> Authors { get; set; }

        public List<CommentOut> Comments { get; set; }

        public List<string> Tags { get; set; }

        public PostOut(Post post)
        {
            Id = post.Id;

            Title = post.Title;
            Resume = post.Resume;
            Body = post.Body;

            CreatedAt = post.CreatedAt;

            Authors = post.Authors?.Select(b => b.Name).ToList();
            Comments = post.Comments?.Select(c => new CommentOut(c)).ToList();
            Tags = post.Tags?.Select(c => c.Name).ToList();
        }
    }
}
