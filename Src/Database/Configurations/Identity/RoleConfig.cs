using Blog.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Database.Configurations.Identity
{
    public class RoleConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> role)
        {
            role.ToTable("roles", "entity");

            role.HasKey(r => r.Id);

            // Each Role can have many entries in the UserRole join table.
            role.HasMany<UserRole>()
                .WithOne()
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            // Each Role can have many associated RoleClaims.
            role.HasMany<RoleClaim>()
                .WithOne()
                .HasForeignKey(rc => rc.RoleId)
                .IsRequired();

            // A concurrency token for use with the optimistic concurrency checking.
            role.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

            // Index for normalized role name to allow efficient lookups.
            role.HasIndex(r => r.NormalizedName).HasDatabaseName("normalized_name_unique_index").IsUnique();
        }
    }
}
