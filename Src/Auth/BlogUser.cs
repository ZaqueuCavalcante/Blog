using Blog.Domain;
using Microsoft.AspNetCore.Identity;

namespace Blog.Auth;

public class BlogUser : IdentityUser<int>
{
    public List<Network> Networks { get; set; }

    public List<RefreshToken> RefreshTokens { get; set; }

    public BlogUser(string email)
    {
        UserName = email;
        Email = email;
    }
}
