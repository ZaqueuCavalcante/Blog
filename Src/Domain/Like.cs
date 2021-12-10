namespace Blog.Domain
{
    public class Like
    {
        public int Id { get; set; }

        public int CommentId { get; set; }

        public int? ReaderId { get; set; }
        public int? BloggerId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
