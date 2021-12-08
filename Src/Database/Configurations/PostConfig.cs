using Blog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Database.Configurations
{
    public class PostConfig : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> post)
        {
            post.ToTable("posts");

            post.HasKey(a => a.Id);

            post.Property(a => a.Title).IsRequired();
            post.Property(a => a.Resume).IsRequired();
            post.Property(b => b.Body).IsRequired();

            post.Property(b => b.CreatedAt).IsRequired();

            post.HasMany<Blogger>(p => p.Authors)
                .WithMany(b => b.Posts)
                .UsingEntity<Dictionary<string, object>>(
                    joinEntityName: "Publications",
                    configureRight: b => b.HasOne<Blogger>().WithMany().HasForeignKey("BloggerId"),
                    configureLeft: b => b.HasOne<Post>().WithMany().HasForeignKey("PostId")
            );

            post.HasMany<Comment>(p => p.Comments)
                .WithOne()
                .HasForeignKey(x => x.PostId);

            post.HasMany<Tag>(p => p.Tags)
                .WithMany(b => b.Posts)
                .UsingEntity<Dictionary<string, object>>(
                    joinEntityName: "Categorizations",
                    configureRight: b => b.HasOne<Tag>().WithMany().HasForeignKey("Name"),
                    configureLeft: b => b.HasOne<Post>().WithMany().HasForeignKey("PostId"))
                .Property("Name").HasColumnName("tag_name");  
        }
    }
}
