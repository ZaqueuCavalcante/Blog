using Blog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Database.Configurations.Domain
{
    public class ReplyConfig : IEntityTypeConfiguration<Reply>
    {
        public void Configure(EntityTypeBuilder<Reply> reply)
        {
            reply.ToTable("replies");

            reply.HasKey(r => r.Id);

            reply.Property(r => r.Body).IsRequired();
            reply.Property(r => r.CreatedAt).IsRequired();

            reply.HasOne<Reader>()
                .WithMany()
                .HasForeignKey(r => r.ReaderId)
                .IsRequired(false);

            reply.HasOne<Blogger>()
                .WithMany()
                .HasForeignKey(r => r.BloggerId)
                .IsRequired(false);

            reply.HasCheckConstraint(
                "replies_must_have_a_single_replier",
                "(reader_id IS NULL) != (blogger_id IS NULL)"
            );
        }
    }
}
