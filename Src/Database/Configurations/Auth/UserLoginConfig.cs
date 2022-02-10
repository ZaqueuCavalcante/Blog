using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Database.Configurations.Identity;

public class UserLoginConfig : IEntityTypeConfiguration<IdentityUserLogin<int>>
{
    public void Configure(EntityTypeBuilder<IdentityUserLogin<int>> userLogin)
    {
        userLogin.ToTable("user_logins", "auth");

        // Composite primary key consisting of the LoginProvider and the key to use with that provider.
        userLogin.HasKey(ul => new { ul.LoginProvider, ul.ProviderKey });

        // Limit the size of the composite key columns due to common DB restrictions.
        userLogin.Property(l => l.LoginProvider).HasMaxLength(128);
        userLogin.Property(l => l.ProviderKey).HasMaxLength(128);
    }
}
