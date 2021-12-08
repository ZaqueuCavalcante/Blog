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
            comment.Property(a => a.PostId).IsRequired();

            comment.Property(a => a.Body).IsRequired();
            comment.Property(a => a.CreatedAt).IsRequired();

            comment.HasOne<Reader>()
                .WithMany()
                .HasForeignKey(x => x.ReaderId)
                .IsRequired(false);

            comment.HasOne<Blogger>()
                .WithMany()
                .HasForeignKey(x => x.BloggerId)
                .IsRequired(false);

            comment.HasMany<Reply>(p => p.Replies)
                .WithOne()
                .HasForeignKey(x => x.CommentId)
                .IsRequired();

            comment.HasCheckConstraint(
                "comments_must_have_a_single_commenter",
                "(reader_id IS NULL) != (blogger_id IS NULL)"
            );
        }
    }
}
