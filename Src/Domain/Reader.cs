using Blog.Exceptions;

namespace Blog.Domain;

public class Reader
{
    public int Id { get; }

    public int UserId { get; set; }

    public string Name { get; private set; }

    public DateTime CreatedAt { get; }

    public Reader(
        string name,
        int userId
    ) {
        SetName(name);
        UserId = userId;
        CreatedAt = DateTime.Now;
    }

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Readers's name cannot be empty.");

        if (name.Count() < 3)
            throw new DomainException("Readers's name cannot be that short.");

        Name = name;
    }
}
