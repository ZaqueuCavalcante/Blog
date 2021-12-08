using Blog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Database.Configurations
{
    public class CommentConfig : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> comment)
        {
            comment.ToTable("comments");

            comment.HasKey(a => a.Id);

            comment.Property(a => a.Body).IsRequired();

            comment.Property(a => a.CreatedAt).IsRequired();

            comment.Property(a => a.PostId).IsRequired(false);

            comment.HasOne<Reader>(p => p.Reader)
                .WithMany()
                .HasForeignKey(x => x.ReaderId)
                .IsRequired(false);

            comment.HasOne<Blogger>(p => p.Blogger)
                .WithMany()
                .HasForeignKey(x => x.BloggerId)
                .IsRequired(false);

            comment.HasMany<Comment>(p => p.Replies)
                .WithOne()
                .HasForeignKey(x => x.RepliedCommentId)
                .IsRequired(false);
        }
    }
}
