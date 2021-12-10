using Blog.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Database.Configurations.Identity
{
    public class RoleClaimConfig : IEntityTypeConfiguration<RoleClaim>
    {
        public void Configure(EntityTypeBuilder<RoleClaim> roleClaim)
        {
            roleClaim.ToTable("role_claims", "entity");

            roleClaim.HasKey(rc => rc.Id);
        }
    }
}
