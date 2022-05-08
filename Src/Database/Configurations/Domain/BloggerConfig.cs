using Blog.Domain;
using Blog.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Database.Configurations.Domain;

public class BloggerConfig : IEntityTypeConfiguration<Blogger>
{
    public void Configure(EntityTypeBuilder<Blogger> blogger)
    {
        blogger.ToTable("bloggers");

        blogger.HasKey(b => b.Id);

        blogger.Property(b => b.Name).IsRequired();

        blogger.Property(b => b.Resume).IsRequired();

        blogger.Property(b => b.CreatedAt).IsRequired();

        blogger.HasOne<BlogUser>()
            .WithOne()
            .HasForeignKey<Blogger>(b => b.UserId)
            .IsRequired();

        blogger.HasMany<Network>(b => b.Networks)
            .WithOne()
            .HasForeignKey(n => n.BloggerId)
            .IsRequired();
    }
}
