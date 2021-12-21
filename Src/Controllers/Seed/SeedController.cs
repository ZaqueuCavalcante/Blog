using System.Security.Claims;
using Blog.Database;
using Blog.Domain;
using Blog.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers.Bloggers
{
    [ApiController]
    [Route("[controller]")]
    public class SeedController : ControllerBase
    {
        private readonly BlogContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public SeedController(
            BlogContext context,
            UserManager<User> userManager,
            RoleManager<Role> roleManager
        ) {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<ActionResult> SeedDb()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            #region Roles and Claims

            var readerRole = new Role { Name = "Reader" };
            var bloggerRole = new Role { Name = "Blogger" };
            var adminRole = new Role { Name = "Admin" };

            await _roleManager.CreateAsync(readerRole);
            await _roleManager.CreateAsync(bloggerRole);
            await _roleManager.CreateAsync(adminRole);

            await _roleManager.AddClaimAsync(adminRole, new Claim("pinner", "true"));

            #endregion
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            #region Users

            var samUser = new User { UserName = "sam@blog.com", Email = "sam@blog.com" };
            var elliotUser = new User { UserName = "elliot@blog.com", Email = "elliot@blog.com" };
            var darleneUser = new User { UserName = "darlene@blog.com", Email = "darlene@blog.com" };
            var tyrellUser = new User { UserName = "tyrell@blog.com", Email = "tyrell@blog.com" };
            var angelaUser = new User { UserName = "angela@blog.com", Email = "angela@blog.com" };

            await _userManager.CreateAsync(samUser, "Test@123");
            await _userManager.CreateAsync(elliotUser, "Test@123");
            await _userManager.CreateAsync(darleneUser, "Test@123");
            await _userManager.CreateAsync(tyrellUser, "Test@123");
            await _userManager.CreateAsync(angelaUser, "Test@123");

            await _userManager.AddToRoleAsync(samUser, "Admin");
            await _userManager.AddToRoleAsync(elliotUser, "Blogger");
            await _userManager.AddToRoleAsync(darleneUser, "Reader");
            await _userManager.AddToRoleAsync(tyrellUser, "Reader");
            await _userManager.AddToRoleAsync(angelaUser, "Reader");

            await _userManager.AddClaimAsync(elliotUser, new Claim("pinner", "true"));

            #endregion
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            #region Bloggers, Tags, Categories and Posts

            var samBlogger = new Blogger("Sam Esmail", "A TV show blogger...", samUser.Id);
            await _context.Bloggers.AddAsync(samBlogger);
            await _context.SaveChangesAsync();

            var elliotBlogger = new Blogger("Sam Sepiol", "A tech blogger...", elliotUser.Id);
            await _context.Bloggers.AddAsync(elliotBlogger);
            await _context.SaveChangesAsync();

            samUser.Networks = new List<Network>
            {
                new Network { Name = "YouTube", Uri = "https://www.youtube.com/sam" },
                new Network { Name = "Twitter", Uri = "https://twitter.com/sam" }
            };

            var techTag = new Tag { Name = "Tech", CreatedAt = DateTime.Now };
            await _context.Tags.AddAsync(techTag);
            await _context.SaveChangesAsync();
            var seriesTag = new Tag { Name = "Series", CreatedAt = DateTime.Now };
            await _context.Tags.AddAsync(seriesTag);
            await _context.SaveChangesAsync();
            var hackingTag = new Tag { Name = "Hacking", CreatedAt = DateTime.Now };
            await _context.Tags.AddAsync(hackingTag);
            await _context.SaveChangesAsync();

            var linuxCategory = new Category
            {
                Name = "Linux",
                Description = "The Linux category description...",
                CreatedAt = DateTime.Now
            };

            await _context.Categories.AddAsync(linuxCategory);
            await _context.SaveChangesAsync();

            var mrRobotCategory = new Category
            {
                Name = "Mr. Robot",
                Description = "The Mr. Robot category description...",
                CreatedAt = DateTime.Now
            };

            await _context.Categories.AddAsync(mrRobotCategory);
            await _context.SaveChangesAsync();

            var mrRobotpost = new Post
            {
                Title = "Mr. Robot - End explained",
                Resume = "A very short blog post resume.",
                Body = "A blog post with many informations...",
                CategoryId = mrRobotCategory.Id,
                CreatedAt = DateTime.Now,
                Authors = new List<Blogger>{ samBlogger, elliotBlogger },
                Tags = new List<Tag>{ seriesTag, hackingTag }
            };

            await _context.Posts.AddAsync(mrRobotpost);
            await _context.SaveChangesAsync();

            var linuxPost = new Post
            {
                Title = "Linux and hacking",
                Resume = "A other very short blog post resume.",
                Body = "A other blog post with many informations...",
                CategoryId = linuxCategory.Id,
                CreatedAt = DateTime.Now,
                Authors = new List<Blogger>{ elliotBlogger },
                Tags = new List<Tag>{ techTag, hackingTag }
            };

            await _context.Posts.AddAsync(linuxPost);
            await _context.SaveChangesAsync();

            #endregion
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            #region Readers

            var elliotReader = new Reader("Elliot Alderson", elliotUser.Id);
            await _context.Readers.AddAsync(elliotReader);
            await _context.SaveChangesAsync();

            var darleneReader = new Reader("Darlene", darleneUser.Id);
            await _context.Readers.AddAsync(darleneReader);
            await _context.SaveChangesAsync();

            var tyrellReader = new Reader("Tyrell Wellick", tyrellUser.Id);
            await _context.Readers.AddAsync(tyrellReader);
            await _context.SaveChangesAsync();

            var angelaReader = new Reader("Angela Moss", angelaUser.Id);
            await _context.Readers.AddAsync(angelaReader);
            await _context.SaveChangesAsync();

            #endregion
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            #region Comments

            var mrRobotPostComment01 = new Comment
            {
                PostId = mrRobotpost.Id,
                PostRating = 5,
                Body = "Great first season.",
                CreatedAt = DateTime.Now,
                UserId = darleneUser.Id
            };

            var mrRobotPostComment02 = new Comment
            {
                PostId = mrRobotpost.Id,
                PostRating = 4,
                Body = "Amazing.",
                CreatedAt = DateTime.Now,
                UserId = tyrellUser.Id
            };

            var mrRobotPostComment03 = new Comment
            {
                PostId = mrRobotpost.Id,
                PostRating = 3,
                Body = "More tath a hacker show.",
                CreatedAt = DateTime.Now,
                UserId = angelaUser.Id
            };

            await _context.Comments.AddRangeAsync(mrRobotPostComment01, mrRobotPostComment02, mrRobotPostComment03);
            await _context.SaveChangesAsync();

            var linuxPostComment01 = new Comment
            {
                PostId = linuxPost.Id,
                PostRating = 5,
                Body = "Very useful.",
                CreatedAt = DateTime.Now,
                UserId = tyrellUser.Id
            };

            var linuxPostComment02 = new Comment
            {
                PostId = linuxPost.Id,
                PostRating = 5,
                Body = "Interesting...",
                CreatedAt = DateTime.Now,
                UserId = samUser.Id
            };

            await _context.Comments.AddRangeAsync(linuxPostComment01, linuxPostComment02);
            await _context.SaveChangesAsync();

            #endregion
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            #region Replies

            var reply01 = new Reply
            {
                CommentId = mrRobotPostComment01.Id,
                Body = "A comment reply...",
                CreatedAt = DateTime.Now,
                UserId = samUser.Id
            };

            var reply02 = new Reply
            {
                CommentId = mrRobotPostComment01.Id,
                Body = "A other comment reply...",
                CreatedAt = DateTime.Now,
                UserId = elliotUser.Id
            };

            var reply03 = new Reply
            {
                CommentId = mrRobotPostComment02.Id,
                Body = "A reply lalala...",
                CreatedAt = DateTime.Now,
                UserId = angelaUser.Id
            };

            await _context.Replies.AddRangeAsync(reply01, reply02, reply03);
            await _context.SaveChangesAsync();

            #endregion
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            #region Likes

            var like01 = new Like
            {
                CommentId = mrRobotPostComment01.Id,
                CreatedAt = DateTime.Now,
                UserId = darleneUser.Id
            };

            var like02 = new Like
            {
                CommentId = mrRobotPostComment01.Id,
                CreatedAt = DateTime.Now,
                UserId = tyrellUser.Id
            };

            var like03 = new Like
            {
                CommentId = mrRobotPostComment01.Id,
                CreatedAt = DateTime.Now,
                UserId = angelaUser.Id
            };

            var like04 = new Like
            {
                CommentId = mrRobotPostComment02.Id,
                CreatedAt = DateTime.Now,
                UserId = tyrellUser.Id
            };

            var like05 = new Like
            {
                CommentId = linuxPostComment01.Id,
                CreatedAt = DateTime.Now,
                UserId = samUser.Id
            };

            var like06 = new Like
            {
                CommentId = linuxPostComment02.Id,
                CreatedAt = DateTime.Now,
                UserId = elliotUser.Id
            };

            await _context.Likes.AddRangeAsync(like01, like02, like03, like04, like05, like06);
            await _context.SaveChangesAsync();

            #endregion

            return Ok("Seed completed!");
        }
    }
}
