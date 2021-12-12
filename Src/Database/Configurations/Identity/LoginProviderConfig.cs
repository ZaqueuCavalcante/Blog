using Blog.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Database.Configurations.Identity
{
    public class LoginProviderConfig : IEntityTypeConfiguration<LoginProvider>
    {
        public void Configure(EntityTypeBuilder<LoginProvider> loginProvider)
        {
            loginProvider.ToTable("login_providers", "entity");

            loginProvider.HasKey(lp => lp.Id);

            // Comentado para facilitar os testes...
            // loginProvider.HasMany<IdentityUserLogin<int>>()
            //     .WithOne()
            //     .HasPrincipalKey(lp => new { lp.Id, lp.Name })
            //     .HasForeignKey(ul => new { ul.LoginProvider, ul.ProviderDisplayName });

            // loginProvider.HasMany<IdentityUserToken<int>>()
            //     .WithOne()
            //     .HasPrincipalKey(lp => lp.Id)
            //     .HasForeignKey(ut => ut.LoginProvider);
        }
    }
}
