namespace Blog.Controllers.Posts
{
    public class LikeOut
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }

        public static LikeOut New(int commentId, int userId)
        {
            return new LikeOut
            {
                CommentId = commentId,
                UserId = userId
            };
        }
    }
}
