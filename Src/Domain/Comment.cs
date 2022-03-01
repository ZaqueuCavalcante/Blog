using Blog.Exceptions;

namespace Blog.Domain;

public class Comment
{
    public int Id { get; }

    public int PostId { get; }

    public int UserId { get; }

    public byte PostRating { get; private set; }

    public string Body { get; private set; }

    public DateTime CreatedAt { get; }

    public List<Reply> Replies { get; }

    public List<Like> Likes { get; }

    public Comment(
        int postId,
        byte postRating,
        string body,
        int userId
    ) {
        PostId = postId;
        SetPostRating(postRating);
        SetBody(body);
        CreatedAt = DateTime.Now;
        UserId = userId;
    }

    private void SetPostRating(byte postRating)
    {
        if (postRating < 1 || postRating > 5)
            throw new DomainException("Post rating out of range. Must be between 1 and 5.");

        PostRating = postRating;
    }

    private void SetBody(string body)
    {
        if (string.IsNullOrWhiteSpace(body))
            throw new DomainException("Comment's body cannot be empty.");

        if (body.Count() < 3)
            throw new DomainException("Comment's body cannot be that short.");

        Body = body;
    }

    public int GetLikes()
    {
        if (Likes == null || !Likes.Any())
            return 0;

        return Likes.Count;
    }
}
