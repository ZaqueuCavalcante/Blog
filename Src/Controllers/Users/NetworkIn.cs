using System.ComponentModel.DataAnnotations;

namespace Blog.Controllers.Users
{
    public class NetworkIn
    {
        [Required]
        public string Name { get; set; }

        /// <example>https://github.com/ZaqueuCavalcante</example>
        [Required]
        public string Url { get; set; }
    }
}
