using System.ComponentModel.DataAnnotations;

namespace Blog.Controllers.Networks
{
    public class NetworkIn
    {
        [Required]
        public string Name { get; set; }

        /// <example>https://github.com/ZaqueuCavalcante</example>
        [Required, Url]
        public string Uri { get; set; }
    }
}
