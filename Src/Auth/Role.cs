using Microsoft.AspNetCore.Identity;

namespace Blog.Auth;

public class Role : IdentityRole<int>
{
    public Role(int id, string name, string concurrencyStamp)
    {
        Id = id;
        Name = name;
        NormalizedName = name.ToUpper();
        ConcurrencyStamp = concurrencyStamp;
    }
}
