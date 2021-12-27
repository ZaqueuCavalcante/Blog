namespace Blog.Controllers.Posts
{
    public class PostIn
    {
        public string Title { get; set; }
        public string Resume { get; set; }
        public string Body { get; set; }
        public int CategoryId { get; set; }
        public List<int>? Tags { get; set; }
    }
}
