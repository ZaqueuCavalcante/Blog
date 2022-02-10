using Blog.Domain;
using Blog.Auth;
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

            reply.HasOne<BlogUser>()
                .WithMany()
                .HasForeignKey(u => u.UserId)
                .IsRequired();
        }
    }
}
