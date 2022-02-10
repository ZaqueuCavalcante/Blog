using Blog.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Blog.Configurations.AuthorizationConfigurations;

namespace Blog.Database.Configurations.Auth;

public class RoleConfig : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> role)
    {
        role.ToTable("roles", "auth");

        role.HasKey(r => r.Id);

        // Each Role can have many entries in the UserRole join table.
        role.HasMany<IdentityUserRole<int>>()
            .WithOne()
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired();

        // Each Role can have many associated RoleClaims.
        role.HasMany<IdentityRoleClaim<int>>()
            .WithOne()
            .HasForeignKey(rc => rc.RoleId)
            .IsRequired();

        // A concurrency token for use with the optimistic concurrency checking.
        role.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

        // Index for normalized role name to allow efficient lookups.
        role.HasIndex(r => r.NormalizedName).HasDatabaseName("normalized_name_unique_index").IsUnique();

        role.HasData(new Role(1, AdminRole));
        role.HasData(new Role(2, BloggerRole));
        role.HasData(new Role(3, ReaderRole));
    }
}
