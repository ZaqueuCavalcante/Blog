using System.ComponentModel.DataAnnotations;

namespace Blog.Controllers.Tags;

public class TagIn
{
    [Required]
    public string Name { get; set; }
}
