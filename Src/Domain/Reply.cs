namespace Blog.Domain
{
    public class Reply
    {
        public int Id { get; set; }

        public int CommentId { get; set; }

        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }

        public int? ReaderId { get; set; }

        public int? BloggerId { get; set; }
    }
}
