using System.ComponentModel.DataAnnotations;

namespace Blog.Controllers.Bloggers
{
    public class BloggerUpdateIn
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Resume { get; set; }
    }
}
