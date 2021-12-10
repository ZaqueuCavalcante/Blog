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

        public byte GetRating()
        {
            if (Comments == null || !Comments.Any())
                return 0;

            return (byte) (Comments.Sum(c => c.PostRating) / Comments.Count);
        }
    }
}
