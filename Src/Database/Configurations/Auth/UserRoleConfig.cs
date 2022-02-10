using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Database.Configurations.Auth;

public class UserRoleConfig : IEntityTypeConfiguration<IdentityUserRole<int>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<int>> userRole)
    {
        userRole.ToTable("user_roles", "auth");

        userRole.HasKey(ur => new { ur.UserId, ur.RoleId });
    }
}
