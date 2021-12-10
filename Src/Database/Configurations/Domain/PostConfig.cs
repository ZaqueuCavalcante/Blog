using Blog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Database.Configurations.Domain
{
    public class PostConfig : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> post)
        {
            post.ToTable("posts");

            post.HasKey(p => p.Id);

            post.Property(p => p.Title).IsRequired();
            post.Property(p => p.Resume).IsRequired();
            post.Property(p => p.Body).IsRequired();

            post.Property(p => p.CreatedAt).IsRequired();

            post.HasMany<Blogger>(p => p.Authors)
                .WithMany(b => b.Posts)
                .UsingEntity<Dictionary<string, object>>(
                    joinEntityName: "Publications",
                    configureRight: b => b.HasOne<Blogger>().WithMany().HasForeignKey("BloggerId"),
                    configureLeft: b => b.HasOne<Post>().WithMany().HasForeignKey("PostId")
            );

            post.HasMany<Comment>(p => p.Comments)
                .WithOne()
                .HasForeignKey(c => c.PostId)
                .IsRequired();

            post.HasMany<Tag>(p => p.Tags)
                .WithMany(t => t.Posts)
                .UsingEntity<Dictionary<string, object>>(
                    joinEntityName: "Categorizations",
                    configureRight: b => b.HasOne<Tag>().WithMany().HasForeignKey("Name"),
                    configureLeft: b => b.HasOne<Post>().WithMany().HasForeignKey("PostId"))
                .Property("Name").HasColumnName("tag_name");  
        }
    }
}
