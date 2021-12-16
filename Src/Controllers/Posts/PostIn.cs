namespace Blog.Controllers.Posts
{
    public class PostIn
    {
        public string Title { get; set; }
        public string Resume { get; set; }
        public string Body { get; set; }

        public string Category { get; set; }

        public List<int> Authors { get; set; }

        public List<string> Tags { get; set; }
    }
}
