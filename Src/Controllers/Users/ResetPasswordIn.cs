using System.ComponentModel.DataAnnotations;

namespace Blog.Controllers.Users
{
    public class ResetPasswordIn
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required, Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; }
    }
}
