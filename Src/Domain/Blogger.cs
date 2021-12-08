namespace Blog.Domain
{
    public class Blogger
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Resume { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<Post> Posts { get; set; }
    }
}
