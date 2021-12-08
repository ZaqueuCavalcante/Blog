using Blog.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Database
{
    public class BlogContext : DbContext
    {
        public DbSet<Blogger> Bloggers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<Reply> Replies { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public BlogContext(DbContextOptions<BlogContext> options) : base(options) {}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("blog");

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
