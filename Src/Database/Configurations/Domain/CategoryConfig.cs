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

            category.HasKey(c => c.Id);

            category.Property(c => c.Name).IsRequired();
            category.HasIndex(c => c.Name).IsUnique();

            category.Property(c => c.Description).IsRequired();

            category.Property(c => c.CreatedAt).IsRequired();

            category.HasMany<Post>(c => c.Posts)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .IsRequired(); 
        }
    }
}
