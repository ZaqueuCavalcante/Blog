using Blog.Domain;
using Blog.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.Database
{
    public class BlogContext : IdentityDbContext<User, Role, int>  //DbContext
    {
        public DbSet<Blogger> Bloggers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<Reply> Replies { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Like> Likes { get; set; }

        public DbSet<User> Users { get; set; }
        // public DbSet<Role> Roles { get; set; }
        // public DbSet<UserRole> UserRoles { get; set; }
        // public DbSet<UserClaim> UserClaims { get; set; }
        // public DbSet<UserLogin> UserLogins { get; set; }
        // public DbSet<UserToken> UserTokens { get; set; }
        // public DbSet<RoleClaim> RoleClaims { get; set; }

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
