namespace Blog.Domain
{
    public class Category
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<Post> Posts { get; set; }
    }
}
