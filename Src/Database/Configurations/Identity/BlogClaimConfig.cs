using Blog.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Database.Configurations.Identity
{
    public class BlogClaimConfig : IEntityTypeConfiguration<BlogClaim>
    {
        public void Configure(EntityTypeBuilder<BlogClaim> blogClaim)
        {
            blogClaim.ToTable("blog_claims", "entity");

            blogClaim.HasKey(bc => bc.Id);

            // Comentado para facilitar os testes...
            // blogClaim.HasMany<IdentityUserClaim<int>>()
            //     .WithOne()
            //     .HasPrincipalKey(bc => new { bc.Type, bc.Value })
            //     .HasForeignKey(uc => new { uc.ClaimType, uc.ClaimValue });

            // blogClaim.HasMany<IdentityRoleClaim<int>>()
            //     .WithOne()
            //     .HasPrincipalKey(bc => new { bc.Type, bc.Value })
            //     .HasForeignKey(uc => new { uc.ClaimType, uc.ClaimValue });
        }
    }
}
