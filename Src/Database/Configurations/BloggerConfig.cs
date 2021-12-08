using Blog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Database.Configurations
{
    public class BloggerConfig : IEntityTypeConfiguration<Blogger>
    {
        public void Configure(EntityTypeBuilder<Blogger> blogger)
        {
            blogger.ToTable("bloggers");

            blogger.HasKey(a => a.Id);

            blogger.Property(a => a.Name).IsRequired();
            blogger.Property(a => a.Resume).IsRequired();
        }
    }
}
