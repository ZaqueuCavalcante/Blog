using System.ComponentModel.DataAnnotations;

namespace Blog.Controllers.Users;

public class RefreshTokenIn
{
    [Required]
    public string AccessToken { get; set; }

    [Required]
    public string RefreshToken { get; set; }
}
