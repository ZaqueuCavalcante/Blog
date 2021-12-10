using Blog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Database.Configurations.Domain
{
    public class CommentConfig : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> comment)
        {
            comment.ToTable("comments");

            comment.HasKey(b => b.Id);
            comment.Property(b => b.PostId).IsRequired();
            comment.Property(b => b.PostRating).IsRequired();

            comment.Property(b => b.Body).IsRequired();
            comment.Property(b => b.CreatedAt).IsRequired();

            comment.HasOne<Reader>()
                .WithMany()
                .HasForeignKey(c => c.ReaderId)
                .IsRequired(false);

            comment.HasOne<Blogger>()
                .WithMany()
                .HasForeignKey(c => c.BloggerId)
                .IsRequired(false);

            comment.HasMany<Reply>(c => c.Replies)
                .WithOne()
                .HasForeignKey(r => r.CommentId)
                .IsRequired();

            comment.HasMany<Like>(c => c.Likes)
                .WithOne()
                .HasForeignKey(l => l.CommentId)
                .IsRequired();

            comment.HasCheckConstraint(
                "comments_must_have_a_single_commenter",
                "(reader_id IS NULL) != (blogger_id IS NULL)"
            );

            comment.HasCheckConstraint(
                "post_ratings_must_be_between_1_and_5",
                "post_rating >= 1 AND post_rating <= 5"
            );
        }
    }
}
