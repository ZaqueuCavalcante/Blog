using System.ComponentModel.DataAnnotations;

namespace Blog.Controllers.Categories
{
    public class CategoryIn
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
