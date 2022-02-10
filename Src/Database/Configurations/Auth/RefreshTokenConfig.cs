using Blog.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Database.Configurations.Auth;

public class RefreshTokenConfig : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> refreshToken)
    {
        refreshToken.ToTable("refresh_tokens", "auth");

        refreshToken.HasKey(rt => rt.Id);
    }
}
