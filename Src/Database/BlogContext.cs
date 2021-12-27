using Blog.Domain;
using Blog.Identity;
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
        public DbSet<Like> Likes { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Network> Networks { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public BlogContext(DbContextOptions<BlogContext> options) : base(options) {}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("blog");

            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
