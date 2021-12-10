namespace Blog.Domain
{
    public class Comment
    {
        public int Id { get; set; }

        public int PostId { get; set; }
        public byte PostRating { get; set; }

        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }

        public int? ReaderId { get; set; }

        public int? BloggerId { get; set; }

        public List<Reply> Replies { get; set; }

        public List<Like> Likes { get; set; }

        public int GetLikes()
        {
            if (Likes == null || !Likes.Any())
                return 0;

            return Likes.Count;
        }
    }
}
