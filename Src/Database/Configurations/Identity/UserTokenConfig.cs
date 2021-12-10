using Blog.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Database.Configurations.Identity
{
    public class UserTokenConfig : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> userToken)
        {
            userToken.ToTable("user_tokens", "entity");

            // Composite primary key consisting of the UserId, LoginProvider and Name.
            userToken.HasKey(ut => new { ut.UserId, ut.LoginProvider, ut.Name });

            // Limit the size of the composite key columns due to common DB restrictions.
            userToken.Property(ut => ut.LoginProvider).HasMaxLength(128);
            userToken.Property(ut => ut.Name).HasMaxLength(128);
        }
    }
}