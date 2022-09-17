using System.ComponentModel.DataAnnotations;

namespace Blog.Controllers.Search;

public class SearchParameters : RequestParameters
{
    [Required]
    public string Thing { get; set; }
}
