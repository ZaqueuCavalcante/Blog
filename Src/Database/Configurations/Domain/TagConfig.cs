using Blog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Database.Configurations.Domain
{
    public class TagConfig : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> tag)
        {
            tag.ToTable("tags");

            tag.HasKey(t => t.Id);

            tag.Property(t => t.Name).IsRequired();
            tag.HasIndex(t => t.Name).IsUnique();

            tag.Property(t => t.CreatedAt).IsRequired();
        }
    }
}
