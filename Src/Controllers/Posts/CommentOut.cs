using Blog.Domain;

namespace Blog.Controllers.Posts
{
    public class CommentOut
    {
        public int Id { get; set; }

        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }

        public int? ReaderId { get; set; }
        public int? BloggerId { get; set; }

        public List<ReplyOut> Replies { get; set; }

        public CommentOut(Comment comment)
        {
            Id = comment.Id;

            Body = comment.Body;
            CreatedAt = comment.CreatedAt;

            ReaderId = comment.ReaderId;
            BloggerId = comment.BloggerId;

            Replies = comment.Replies?.Select(b => new ReplyOut(b)).ToList();
        }
    }
}
