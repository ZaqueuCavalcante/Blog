using Blog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Database.Configurations
{
    public class ReaderConfig : IEntityTypeConfiguration<Reader>
    {
        public void Configure(EntityTypeBuilder<Reader> reader)
        {
            reader.ToTable("readers");

            reader.HasKey(a => a.Id);

            reader.Property(a => a.Name).IsRequired();
        }
    }
}
