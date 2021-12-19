using Blog.Domain;
using Blog.Extensions;

namespace Blog.Controllers.Posts
{
    public class ReplyOut
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Body { get; set; }
        public string CreatedAt { get; set; }

        public static ReplyOut New(Reply reply)
        {
            return new ReplyOut
            {
                Id = reply.Id,
                UserId = reply.UserId,
                Body = reply.Body,
                CreatedAt = reply.CreatedAt.Format()
            };
        }
    }
}
