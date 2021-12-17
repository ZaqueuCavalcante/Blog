namespace Blog.Domain
{
    public class Tag
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<Post> Posts { get; set; }
    }
}
