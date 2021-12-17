namespace Blog.Domain
{
    public class Reply
    {
        public int Id { get; set; }

        public int CommentId { get; set; }
        
        public int UserId { get; set; }

        public string Body { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
