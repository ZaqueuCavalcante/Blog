using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Database.Configurations.Identity
{
    public class UserClaimConfig : IEntityTypeConfiguration<IdentityUserClaim<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserClaim<int>> userClaim)
        {
            userClaim.ToTable("user_claims", "identity");

            userClaim.HasKey(uc => uc.Id);
        }
    }
}
