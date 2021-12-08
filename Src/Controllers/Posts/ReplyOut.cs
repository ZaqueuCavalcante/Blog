using Blog.Domain;

namespace Blog.Controllers.Posts
{
    public class ReplyOut
    {
        public int Id { get; set; }

        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }

        public int? ReaderId { get; set; }
        public int? BloggerId { get; set; }

        public ReplyOut(Reply reply)
        {
            Id = reply.Id;

            Body = reply.Body;
            CreatedAt = reply.CreatedAt;

            ReaderId = reply.ReaderId;
            BloggerId = reply.BloggerId;
        }
    }
}
