using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Database.Configurations.Auth;

public class UserTokenConfig : IEntityTypeConfiguration<IdentityUserToken<int>>
{
    public void Configure(EntityTypeBuilder<IdentityUserToken<int>> userToken)
    {
        userToken.ToTable("user_tokens", "auth");

        // Composite primary key consisting of the UserId, LoginProvider and Name.
        userToken.HasKey(ut => new { ut.UserId, ut.LoginProvider, ut.Name });

        // Limit the size of the composite key columns due to common DB restrictions.
        userToken.Property(ut => ut.LoginProvider).HasMaxLength(128);
        userToken.Property(ut => ut.Name).HasMaxLength(128);
    }
}
