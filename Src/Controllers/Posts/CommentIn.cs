namespace Blog.Controllers.Posts
{
    public class CommentIn
    {
        public string Body { get; set; }

        public int? ReaderId { get; set; }
        public int? BloggerId { get; set; }

        public int? RepliedCommentId { get; set; }
    }
}
