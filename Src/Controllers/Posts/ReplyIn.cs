namespace Blog.Controllers.Posts
{
    public class ReplyIn
    {
        public int? ReaderId { get; set; }
        public int? BloggerId { get; set; }

        public string Body { get; set; }
    }
}
