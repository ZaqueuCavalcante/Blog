using System.ComponentModel.DataAnnotations;

namespace Blog.Controllers.Posts
{
    public class ReplyIn
    {
        [Required]
        public string Body { get; set; }
    }
}
