namespace Blog.Domain
{
    public class Post
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Resume { get; set; }
        public string Body { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<Blogger> Authors { get; set; }

        public List<Comment> Comments { get; set; }

        public List<Tag> Tags { get; set; }
    }
}
