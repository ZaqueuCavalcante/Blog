using Blog.Domain;
using Microsoft.AspNetCore.Identity;

namespace Blog.Identity
{
    public class User : IdentityUser<int>
    {
        public List<Network> Networks { get; set; }
    }
}
