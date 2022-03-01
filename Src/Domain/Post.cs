using Blog.Exceptions;

namespace Blog.Domain;

public class Post
{
    public int Id { get; }

    public int AuthorId { get; }
    public Blogger Author { get; }

    public int CategoryId { get; }
    public Category Category { get; }

    public int? PinnedCommentId { get; private set; }

    public string Title { get; private set; }

    public string Resume { get; private set; }

    public string Body { get; private set; }  // TODO: how add images, code and visual things here? Like a Markdown file...

    public DateTime CreatedAt { get; }

    public DateTime? UpdatedAt { get; private set; }

    public List<Comment> Comments { get; }

    public List<Tag> Tags { get; }

    public Post() {}

    public Post(
        string title,
        string resume,
        string body,
        int categoryId,
        int authorId,
        List<Tag>? tags = null
    ) {
        SetTitle(title);
        SetResume(resume);
        SetBody(body);
        CategoryId = categoryId;
        CreatedAt = DateTime.Now;
        AuthorId = authorId;
        Tags = tags;
    }

    public byte GetRating()
    {
        if (Comments == null || !Comments.Any())
            return 0;

        return (byte) (Comments.Sum(c => c.PostRating) / Comments.Count);
    }

    public void Edit(
        string title,
        string resume,
        string body
    ) {
        SetTitle(title);
        SetResume(resume);
        SetBody(body);
        UpdatedAt = DateTime.Now;
    }

    public void Pin(int commentId)
    {
        PinnedCommentId = (PinnedCommentId == commentId) ? null : commentId;
    }

    private void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Post's title cannot be empty.");

        if (title.Count() < 5)
            throw new DomainException("Post's title cannot be that short.");

        Title = title;
    }

    private void SetResume(string resume)
    {
        if (string.IsNullOrWhiteSpace(resume))
            throw new DomainException("Post's resume cannot be empty.");

        if (resume.Count() < 20)
            throw new DomainException("Post's resume cannot be that short.");

        Resume = resume;
    }

    private void SetBody(string body)
    {
        if (string.IsNullOrWhiteSpace(body))
            throw new DomainException("Post's body cannot be empty.");

        if (body.Count() < 100)
            throw new DomainException("Post's body cannot be that short.");

        Body = body;
    }
}
