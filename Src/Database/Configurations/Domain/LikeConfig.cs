using Blog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Database.Configurations.Domain
{
    public class LikeConfig : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> like)
        {
            like.ToTable("likes");

            like.HasKey(l => l.Id);

            like.Property(l => l.CommentId).IsRequired();

            like.Property(l => l.CreatedAt).IsRequired();

            like.HasOne<Reader>()
                .WithMany()
                .HasForeignKey(l => l.ReaderId)
                .IsRequired(false);

            like.HasOne<Blogger>()
                .WithMany()
                .HasForeignKey(l => l.BloggerId)
                .IsRequired(false);

            like.HasCheckConstraint(
                "likes_must_have_a_single_liker",
                "(reader_id IS NULL) != (blogger_id IS NULL)"
            );

            like.HasIndex(l => new { l.CommentId, l.ReaderId })
                .IsUnique()
                .HasDatabaseName("a_like_must_have_a_single_reader");

            like.HasIndex(l => new { l.CommentId, l.BloggerId })
                .IsUnique()
                .HasDatabaseName("a_like_must_have_a_single_blogger");
        }
    }
}
