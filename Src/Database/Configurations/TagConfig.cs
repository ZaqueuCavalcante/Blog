using Blog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Database.Configurations
{
    public class TagConfig : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> tag)
        {
            tag.ToTable("tags");

            tag.HasKey(a => a.Name).HasName("Name");

            tag.Property(a => a.CreatedAt).IsRequired();
        }
    }
}
