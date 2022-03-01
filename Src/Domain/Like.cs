namespace Blog.Domain;

public class Like
{
    public int Id { get; }

    public int CommentId { get; }

    public int UserId { get; }

    public DateTime CreatedAt { get; }

    public Like(
        int commentId,
        int userId
    ) {
        CommentId = commentId;
        UserId = userId;
        CreatedAt = DateTime.Now;
    }
}
