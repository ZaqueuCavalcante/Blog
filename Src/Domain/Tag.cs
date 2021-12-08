namespace Blog.Domain
{
    public class Tag
    {
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<Post> Posts { get; set; }
    }
}
