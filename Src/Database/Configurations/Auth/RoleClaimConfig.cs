using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Database.Configurations.Auth;

public class RoleClaimConfig : IEntityTypeConfiguration<IdentityRoleClaim<int>>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<int>> roleClaim)
    {
        roleClaim.ToTable("role_claims", "auth");

        roleClaim.HasKey(rc => rc.Id);
    }
}
