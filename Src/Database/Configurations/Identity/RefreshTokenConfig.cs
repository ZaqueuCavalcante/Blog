using Blog.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Database.Configurations.Identity
{
    public class RefreshTokenConfig : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> refreshToken)
        {
            refreshToken.ToTable("refresh_tokens", "identity");

            refreshToken.HasKey(rt => rt.Id);
        }
    }
}
