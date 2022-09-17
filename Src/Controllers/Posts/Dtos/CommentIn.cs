using System.ComponentModel.DataAnnotations;

namespace Blog.Controllers.Posts;

public class CommentIn
{
    [Required]
    public string Body { get; set; }

    [Required]
    public byte PostRating { get; set; }
}
