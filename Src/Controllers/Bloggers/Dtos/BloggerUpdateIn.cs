using System.ComponentModel.DataAnnotations;

namespace Blog.Controllers.Bloggers;

public class BloggerUpdateIn
{
    /// <example>
    /// Tyler Durden
    /// </example>
    [Required]
    public string Name { get; set; }

    /// <example>
    /// The Narrator's split personality.
    /// </example>
    [Required]
    public string Resume { get; set; }
}
