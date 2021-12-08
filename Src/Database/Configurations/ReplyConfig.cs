using Blog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Database.Configurations
{
    public class ReplyConfig : IEntityTypeConfiguration<Reply>
    {
        public void Configure(EntityTypeBuilder<Reply> reply)
        {
            reply.ToTable("replies");

            reply.HasKey(a => a.Id);

            reply.Property(a => a.Body).IsRequired();
            reply.Property(a => a.CreatedAt).IsRequired();

            reply.HasOne<Reader>()
                .WithMany()
                .HasForeignKey(x => x.ReaderId)
                .IsRequired(false);

            reply.HasOne<Blogger>()
                .WithMany()
                .HasForeignKey(x => x.BloggerId)
                .IsRequired(false);

            reply.HasCheckConstraint(
                "replies_must_have_a_single_replier",
                "(reader_id IS NULL) != (blogger_id IS NULL)"
            );
        }
    }
}
