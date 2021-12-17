using Blog.Domain;
using Blog.Identity;
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

            comment.HasOne<User>()
                .WithMany()
                .HasForeignKey(c => c.UserId)
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
                "post_ratings_must_be_between_1_and_5",
                "post_rating >= 1 AND post_rating <= 5"
            );
        }
    }
}
