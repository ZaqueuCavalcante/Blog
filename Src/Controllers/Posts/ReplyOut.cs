using Blog.Domain;
using Blog.Extensions;

namespace Blog.Controllers.Posts
{
    public class ReplyOut
    {
        public int Id { get; set; }

        public string Body { get; set; }

        public string CreatedAt { get; set; }

        public int UserId { get; set; }

        public ReplyOut(Reply reply)
        {
            Id = reply.Id;
            Body = reply.Body;
            CreatedAt = reply.CreatedAt.Format();;
            UserId = reply.UserId;
        }
    }
}
