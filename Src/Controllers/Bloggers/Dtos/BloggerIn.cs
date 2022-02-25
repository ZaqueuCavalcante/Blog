using System.ComponentModel.DataAnnotations;

namespace Blog.Controllers.Bloggers;

public class BloggerIn
{
    /// <example>
    /// Bob Marley
    /// </example>
    [Required]
    public string Name { get; set; }

    /// <example>
    /// I write about things.
    /// </example>
    [Required]
    public string Resume { get; set; }

    /// <example>
    /// bob@blog.com
    /// </example>
    [Required, EmailAddress]
    public string Email { get; set; }

    /// <example>
    /// Test@123
    /// </example>
    [Required]
    public string Password { get; set; }
}
