using Microsoft.AspNetCore.Identity;

namespace Blog.Auth;

public class BlogRole : IdentityRole<int>
{
    public BlogRole(int id, string name, string concurrencyStamp)
    {
        Id = id;
        Name = name;
        NormalizedName = name.ToUpper();
        ConcurrencyStamp = concurrencyStamp;
    }
}
