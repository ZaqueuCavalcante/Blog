using Blog.Domain;
using Blog.Extensions;

namespace Blog.Controllers.Posts
{
    public class CommentOut
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public byte PostRating { get; set; }

        public int Likes { get; set; }

        public string Body { get; set; }

        public string CreatedAt { get; set; }

        public List<ReplyOut> Replies { get; set; }

        public CommentOut(Comment comment)
        {
            Id = comment.Id;
            UserId = comment.UserId;
            PostRating = comment.PostRating;
            Likes = comment.GetLikes();
            Body = comment.Body;
            CreatedAt = comment.CreatedAt.Format();
            Replies = comment.Replies?.Select(b => new ReplyOut(b)).ToList();
        }
    }
}
