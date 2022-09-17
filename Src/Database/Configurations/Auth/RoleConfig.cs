using Blog.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Blog.Configurations.AuthorizationConfigurations;

namespace Blog.Database.Configurations.Auth;

public class RoleConfig : IEntityTypeConfiguration<BlogRole>
{
    public void Configure(EntityTypeBuilder<BlogRole> role)
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

        role.HasData(new BlogRole(1, AdminRole, "58923637-168f-4960-8661-a17a44b2eda4"));
        role.HasData(new BlogRole(2, BloggerRole, "2702e8cf-880d-4041-809f-9bf398e09e6d"));
        role.HasData(new BlogRole(3, ReaderRole, "0f32a835-b581-4fb9-a4af-ecbb95b1850c"));
    }
}
