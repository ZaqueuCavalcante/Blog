using Blog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Database.Configurations.Domain
{
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> category)
        {
            category.ToTable("categories");

            category.HasKey(c => c.Name);

            category.Property(c => c.Description).IsRequired();

            category.Property(c => c.CreatedAt).IsRequired();

            category.HasMany<Post>(c => c.Posts)
                .WithOne()
                .HasPrincipalKey(c => c.Name)
                .HasForeignKey(p => p.Category)
                .IsRequired(); 
        }
    }
}
