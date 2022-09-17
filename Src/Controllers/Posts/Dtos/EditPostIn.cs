using System.ComponentModel.DataAnnotations;

namespace Blog.Controllers.Posts;

public class EditPostIn
{
    [Required]
    public string Title { get; set; }

    [Required]
    public string Resume { get; set; }

    [Required]
    public string Body { get; set; }
}
