using Blog.Exceptions;

namespace Blog.Domain;

public class Network
{
    public int Id { get; }

    public int BloggerId { get; }

    public string Name { get; private set; }

    public string Uri { get; private set; }

    public Network(
        int bloggerId,
        string name,
        string uri
    ) {
        BloggerId = bloggerId;
        SetName(name);
        SetUri(uri);
    }

    public Network(
        string name,
        string uri
    ) {
        SetName(name);
        SetUri(uri);
    }

    private void SetName(string name)
    {
        if (!Alloweds.Contains(name))
            throw new DomainException("Invalid network name.");

        Name = name;
    }

    public void SetUri(string uri)
    {
        if (!System.Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            throw new DomainException("Invalid network uri.");

        Uri = uri;
    }

    public static readonly HashSet<string> Alloweds = new HashSet<string>
    {
        { "GitHub" }, { "Twitter" }, { "YouTube" }, { "Instagram" }, { "LinkedIn" }
    };
}
