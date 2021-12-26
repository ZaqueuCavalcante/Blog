using System.ComponentModel.DataAnnotations;

namespace Blog.Controllers.Bloggers
{
    public class BloggerIn
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Resume { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
