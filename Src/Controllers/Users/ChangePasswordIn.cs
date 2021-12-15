using System.ComponentModel.DataAnnotations;

namespace Blog.Controllers.Users
{
    public class ChangePasswordIn
    {
        [Required]
        public string Current { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        [Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; }
    }
}
