using Blog.Domain;
using Microsoft.AspNetCore.Identity;

namespace Blog.Identity
{
    public class User : IdentityUser<int>
    {
        public List<Network> Networks { get; set; }

        public List<RefreshToken> RefreshTokens { get; set; }

        public static User New(string email)
        {
            return new User
            {
                UserName = email,
                Email = email
            };
        }
    }
}
