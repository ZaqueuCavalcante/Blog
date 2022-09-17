namespace Blog.Controllers.Posts;

public class LikeOut
{
    public int CommentId { get; set; }
    public int UserId { get; set; }

    public LikeOut(int commentId, int userId)
    {
        CommentId = commentId;
        UserId = userId;
    }
}
