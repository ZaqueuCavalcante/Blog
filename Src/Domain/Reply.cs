using Blog.Exceptions;

namespace Blog.Domain;

public class Reply
{
    public int Id { get; }

    public int CommentId { get; }

    public int UserId { get; }

    public string Body { get; private set; }

    public DateTime CreatedAt { get; }

    public Reply(
        int commentId,
        string body,
        int userId
    ) {
        CommentId = commentId;
        SetBody(body);
        UserId = userId;
        CreatedAt = DateTime.Now;
    }

    private void SetBody(string body)
    {
        if (string.IsNullOrWhiteSpace(body))
            throw new DomainException("Reply's body cannot be empty.");

        if (body.Count() < 3)
            throw new DomainException("Reply's body cannot be that short.");

        Body = body;
    }
}
