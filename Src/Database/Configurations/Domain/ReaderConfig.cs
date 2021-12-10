using Blog.Domain;
using Blog.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Database.Configurations.Domain
{
    public class ReaderConfig : IEntityTypeConfiguration<Reader>
    {
        public void Configure(EntityTypeBuilder<Reader> reader)
        {
            reader.ToTable("readers");

            reader.HasKey(r => r.Id);

            reader.Property(r => r.Name).IsRequired();

            reader.Property(r => r.CreatedAt).IsRequired();

            reader.HasOne<User>()
                .WithOne()
                .HasForeignKey<Reader>(r => r.UserId)
                .IsRequired();
        }
    }
}
