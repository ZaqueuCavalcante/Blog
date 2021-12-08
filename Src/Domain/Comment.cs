namespace Blog.Domain
{
    public class Comment
    {
        public int Id { get; set; }

        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }

        public int? PostId { get; set; }

        public int? ReaderId { get; set; }
        public Reader Reader { get; set; }

        public int? BloggerId { get; set; }
        public Blogger Blogger { get; set; }

        public int? RepliedCommentId { get; set; }
        public List<Comment> Replies { get; set; }
    }
}
