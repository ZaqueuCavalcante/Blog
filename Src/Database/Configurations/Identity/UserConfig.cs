using Blog.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Database.Configurations.Identity
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> user)
        {
            user.ToTable("users", "entity");

            user.HasKey(u => u.Id);

            // Each User can have many UserClaims.
            user.HasMany<UserClaim>()
                .WithOne()
                .HasForeignKey(uc => uc.UserId)
                .IsRequired();

            // Each User can have many UserLogins.
            user.HasMany<UserLogin>()
                .WithOne()
                .HasForeignKey(ul => ul.UserId)
                .IsRequired();

            // Each User can have many UserTokens.
            user.HasMany<UserToken>()
                .WithOne()
                .HasForeignKey(ut => ut.UserId)
                .IsRequired();

            // Each User can have many entries in the UserRole join table.
            user.HasMany<UserRole>()
                .WithOne()
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            // A concurrency token for use with the optimistic concurrency checking.
            user.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

            // Indexes for normalized username and email, to allow efficient lookups.
            user.HasIndex(u => u.NormalizedUserName).HasDatabaseName("normalized_user_name_unique_index").IsUnique();
            user.HasIndex(u => u.NormalizedEmail).HasDatabaseName("normalized_email_index");
        }
    }
}
