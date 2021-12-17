using Blog.Domain;
using Blog.Identity;
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

            like.HasOne<User>()
                .WithMany()
                .HasForeignKey(l => l.UserId)
                .IsRequired();
        }
    }
}
