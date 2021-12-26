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

            #region Roles

            var readerRole = new Role { Name = "Reader" };
            var bloggerRole = new Role { Name = "Blogger" };
            var adminRole = new Role { Name = "Admin" };

            await _roleManager.CreateAsync(readerRole);
            await _roleManager.CreateAsync(bloggerRole);
            await _roleManager.CreateAsync(adminRole);

            #endregion
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            #region Users

            var samUser = Blog.Identity.User.New("sam@blog.com");
            var elliotUser = Blog.Identity.User.New("elliot@blog.com");
            var irvingUser = Blog.Identity.User.New("irving@blog.com");
            var darleneUser = Blog.Identity.User.New("darlene@blog.com");
            var tyrellUser = Blog.Identity.User.New("tyrell@blog.com");
            var angelaUser = Blog.Identity.User.New("angela@blog.com");
            var domUser = Blog.Identity.User.New("dom@blog.com");

            await _userManager.CreateAsync(samUser, "Test@123");
            await _userManager.CreateAsync(elliotUser, "Test@123");
            await _userManager.CreateAsync(irvingUser, "Test@123");
            await _userManager.CreateAsync(darleneUser, "Test@123");
            await _userManager.CreateAsync(tyrellUser, "Test@123");
            await _userManager.CreateAsync(angelaUser, "Test@123");
            await _userManager.CreateAsync(domUser, "Test@123");

            await _userManager.AddToRolesAsync(samUser, new [] { "Admin", "Blogger" });
            await _userManager.AddToRoleAsync(elliotUser, "Blogger");
            await _userManager.AddToRoleAsync(irvingUser, "Blogger");
            await _userManager.AddToRoleAsync(darleneUser, "Reader");
            await _userManager.AddToRoleAsync(tyrellUser, "Reader");
            await _userManager.AddToRoleAsync(angelaUser, "Reader");
            await _userManager.AddToRoleAsync(domUser, "Reader");

            #endregion
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            #region Claims

            await _userManager.AddClaimAsync(elliotUser, new Claim("pinner", "true"));
            await _userManager.AddClaimAsync(irvingUser, new Claim("liker", "true"));
            await _userManager.AddClaimAsync(darleneUser, new Claim("liker", "true"));

            #endregion
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            #region Bloggers
            
            var samBlogger = new Blogger("Sam Esmail", "Writes about ASP.NET Core, DevOps and TV Shows.", samUser.Id);
            await _context.Bloggers.AddAsync(samBlogger);
            await _context.SaveChangesAsync();

            var elliotBlogger = new Blogger("Elliot Alderson", "Writes about Linux, Hacking and Computers.", elliotUser.Id);
            await _context.Bloggers.AddAsync(elliotBlogger);
            await _context.SaveChangesAsync();

            var irvingBlogger = new Blogger("Irving", "A used-car salesman, that writes novels.", irvingUser.Id);
            await _context.Bloggers.AddAsync(irvingBlogger);
            await _context.SaveChangesAsync();

            #endregion
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            #region Networks

            samUser.Networks = new List<Network>
            {
                new Network { Name = "YouTube", Uri = "https://www.youtube.com/sam" },
                new Network { Name = "Twitter", Uri = "https://twitter.com/sam" }
            };

            irvingUser.Networks = new List<Network>
            {
                new Network { Name = "YouTube", Uri = "https://www.youtube.com/irving" },
                new Network { Name = "Twitter", Uri = "https://twitter.com/irving" }
            };

            await _context.SaveChangesAsync();

            #endregion
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            #region Tags

            var techTag = new Tag { Name = "Tech", CreatedAt = DateTime.Now };
            await _context.Tags.AddAsync(techTag);
            await _context.SaveChangesAsync();

            var seriesTag = new Tag { Name = "Series", CreatedAt = DateTime.Now };
            await _context.Tags.AddAsync(seriesTag);
            await _context.SaveChangesAsync();

            var hackingTag = new Tag { Name = "Hacking", CreatedAt = DateTime.Now };
            await _context.Tags.AddAsync(hackingTag);
            await _context.SaveChangesAsync();

            #endregion
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            #region Categories

            var linuxCategory = new Category("Linux", "The Linux category description.");
            await _context.Categories.AddAsync(linuxCategory);
            await _context.SaveChangesAsync();

            var mrRobotCategory = new Category("Mr. Robot", "The Mr. Robot category description.");
            await _context.Categories.AddAsync(mrRobotCategory);
            await _context.SaveChangesAsync();

            var efCoreCategory = new Category("EF Core", "The EF Core category description.");
            await _context.Categories.AddAsync(efCoreCategory);
            await _context.SaveChangesAsync();

            #endregion
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            #region Posts

            var mrRobotpost = new Post
            {
                Title = "Mr. Robot - End explained",
                Resume = "The Mr. Robot series finale features a stunning, emotional twist that recontextualizes the entire series.",
                Body = "Obviously he is a fictional character given that he exists on the television program Mr. Robot. But even within the reality of the show, Elliot at times more closely resembled a hacker archetype than a living, breathing, bleeding human being.",
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
                Resume = "Linux Basics for Hackers: Getting Started with Networking, Scripting, and Security in Kali.",
                Body = "This practical, tutorial-style book uses the Kali Linux distribution to teach Linux basics with a focus on how hackers would use them. Topics include Linux command line basics, filesystems, networking, BASH basics, package management, logging, and the Linux kernel and drivers.",
                CategoryId = linuxCategory.Id,
                CreatedAt = DateTime.Now,
                Authors = new List<Blogger>{ elliotBlogger },
                Tags = new List<Tag>{ techTag, hackingTag }
            };
            await _context.Posts.AddAsync(linuxPost);
            await _context.SaveChangesAsync();

            var efCorePost = new Post
            {
                Title = "EF Core - Code First",
                Resume = "Code first approach offers most control over the final appearance of the application code and the resulting database.",
                Body = "In EF Core, the DbContext has a virtual method called onConfiguring which will get called internally by EF Core, and it will also pass in an optionsBuilder instance, and you can use that optionsBuilder to configure options for the DbContext.",
                CategoryId = efCoreCategory.Id,
                CreatedAt = DateTime.Now,
                Authors = new List<Blogger>{ irvingBlogger },
                Tags = new List<Tag>{ techTag }
            };
            await _context.Posts.AddAsync(efCorePost);
            await _context.SaveChangesAsync();

            #endregion
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            #region Readers

            var darleneReader = new Reader("Darlene Alderson", darleneUser.Id);
            await _context.Readers.AddAsync(darleneReader);
            await _context.SaveChangesAsync();

            var tyrellReader = new Reader("Tyrell Wellick", tyrellUser.Id);
            await _context.Readers.AddAsync(tyrellReader);
            await _context.SaveChangesAsync();

            var angelaReader = new Reader("Angela Moss", angelaUser.Id);
            await _context.Readers.AddAsync(angelaReader);
            await _context.SaveChangesAsync();

            var domReader = new Reader("Dominique DiPierro", domUser.Id);
            await _context.Readers.AddAsync(domReader);
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
                Body = "A comment reply.",
                CreatedAt = DateTime.Now,
                UserId = samUser.Id
            };

            var reply02 = new Reply
            {
                CommentId = mrRobotPostComment01.Id,
                Body = "A other comment reply.",
                CreatedAt = DateTime.Now,
                UserId = elliotUser.Id
            };

            var reply03 = new Reply
            {
                CommentId = mrRobotPostComment02.Id,
                Body = "A reply lalala.",
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
