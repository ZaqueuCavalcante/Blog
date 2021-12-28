using System.ComponentModel.DataAnnotations;

namespace Blog.Controllers.Users
{
    public class UserIn
    {
        /// <example>sam@blog.com</example>
        [Required, EmailAddress]
        public string Email { get; set; }

        /// <example>Test@123</example>
        [Required]
        public string Password { get; set; }
    }
}
