using System.ComponentModel.DataAnnotations;

namespace Blog.Controllers.Posts
{
    public class PostIn
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Resume { get; set; }

        [Required]
        public string Body { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public List<int>? Tags { get; set; }
    }
}
