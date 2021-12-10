using Blog.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Database.Configurations.Identity
{
    public class UserRoleConfig : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> userRole)
        {
            userRole.ToTable("user_roles", "entity");

            userRole.HasKey(ur => new { ur.UserId, ur.RoleId });
        }
    }
}
