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

        public SeedController(
            BlogContext context,
            UserManager<User> userManager
        ) {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult> SeedDb()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            #region Users

            var samUser = new User { UserName = "sam@blog.com", Email = "sam@blog.com" };
            var elliotUser = new User { UserName = "elliot@blog.com", Email = "elliot@blog.com" };
            var darleneUser = new User { UserName = "darlene@blog.com", Email = "darlene@blog.com" };
            var tyrelUser = new User { UserName = "tyrel@blog.com", Email = "tyrel@blog.com" };
            var angelaUser = new User { UserName = "angela@blog.com", Email = "angela@blog.com" };

            await _userManager.CreateAsync(samUser, "Test@123");
            await _userManager.CreateAsync(elliotUser, "Test@123");
            await _userManager.CreateAsync(darleneUser, "Test@123");
            await _userManager.CreateAsync(tyrelUser, "Test@123");
            await _userManager.CreateAsync(angelaUser, "Test@123");

            #endregion
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            #region Bloggers, Tags, Categories and Posts

            var samBlogger = new Blogger("Sam Esmail", "A TV show blogger...", samUser.Id);
            var elliotBlogger = new Blogger("Sam Sepiol", "A tech blogger...", elliotUser.Id);

            samBlogger.Networks = new List<Network>
            {
                new Network { Name = "YouTube", Uri = "https://www.youtube.com/sam" },
                new Network { Name = "Twitter", Uri = "https://twitter.com/sam" }
            };

            var techTag = new Tag { Name = "Tech", CreatedAt = DateTime.Now };
            var seriesTag = new Tag { Name = "Series", CreatedAt = DateTime.Now };
            var hackingTag = new Tag { Name = "Hacking", CreatedAt = DateTime.Now };

            var linuxCategory = new Category
            {
                Name = "Linux",
                Description = "The Linux category description...",
                CreatedAt = DateTime.Now
            };

            var mrRobotCategory = new Category
            {
                Name = "Mr. Robot",
                Description = "The Mr. Robot category description...",
                CreatedAt = DateTime.Now
            };

            var mrRobotpost = new Post
            {
                Title = "Mr. Robot - End explained",
                Resume = "A very short blog post resume.",
                Body = "A blog post with many informations...",
                Category = mrRobotCategory.Name,
                CreatedAt = DateTime.Now,
                Authors = new List<Blogger>{ samBlogger, elliotBlogger },
                Tags = new List<Tag>{ seriesTag, hackingTag }
            };

            var linuxPost = new Post
            {
                Title = "Linux and hacking",
                Resume = "A other very short blog post resume.",
                Body = "A other blog post with many informations...",
                Category = linuxCategory.Name,
                CreatedAt = DateTime.Now,
                Authors = new List<Blogger>{ elliotBlogger },
                Tags = new List<Tag>{ techTag, hackingTag }
            };

            await _context.Bloggers.AddRangeAsync(samBlogger, elliotBlogger);
            await _context.Categories.AddRangeAsync(linuxCategory, mrRobotCategory);
            await _context.Posts.AddRangeAsync(mrRobotpost, linuxPost);
            await _context.SaveChangesAsync();

            #endregion
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            #region Readers

            var elliotReader = new Reader("Elliot Alderson", elliotUser.Id);
            var darleneReader = new Reader("Darlene", darleneUser.Id);
            var tyrellReader = new Reader("Tyrell Wellick", tyrelUser.Id);
            var angelaReader = new Reader("Angela Moss", angelaUser.Id);

            await _context.Readers.AddRangeAsync(elliotReader, darleneReader, tyrellReader, angelaReader);
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
                ReaderId = darleneReader.Id
            };

            var mrRobotPostComment02 = new Comment
            {
                PostId = mrRobotpost.Id,
                PostRating = 4,
                Body = "Amazing.",
                CreatedAt = DateTime.Now,
                ReaderId = tyrellReader.Id
            };

            var mrRobotPostComment03 = new Comment
            {
                PostId = mrRobotpost.Id,
                PostRating = 3,
                Body = "More tath a hacker show.",
                CreatedAt = DateTime.Now,
                ReaderId = angelaReader.Id
            };

            var linuxPostComment01 = new Comment
            {
                PostId = linuxPost.Id,
                PostRating = 5,
                Body = "Very useful.",
                CreatedAt = DateTime.Now,
                ReaderId = tyrellReader.Id
            };

            var linuxPostComment02 = new Comment
            {
                PostId = linuxPost.Id,
                PostRating = 5,
                Body = "Interesting...",
                CreatedAt = DateTime.Now,
                BloggerId = samBlogger.Id
            };

            await _context.Comments.AddRangeAsync(mrRobotPostComment01, mrRobotPostComment02, mrRobotPostComment03, linuxPostComment01, linuxPostComment02);
            await _context.SaveChangesAsync();

            #endregion
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            #region Replies

            var reply01 = new Reply
            {
                CommentId = mrRobotPostComment01.Id,
                Body = "A comment reply...",
                CreatedAt = DateTime.Now,
                BloggerId = samBlogger.Id
            };

            var reply02 = new Reply
            {
                CommentId = mrRobotPostComment01.Id,
                Body = "A other comment reply...",
                CreatedAt = DateTime.Now,
                BloggerId = elliotBlogger.Id
            };

            var reply03 = new Reply
            {
                CommentId = mrRobotPostComment02.Id,
                Body = "A reply lalala...",
                CreatedAt = DateTime.Now,
                ReaderId = angelaReader.Id
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
                ReaderId = darleneReader.Id
            };

            var like02 = new Like
            {
                CommentId = mrRobotPostComment01.Id,
                CreatedAt = DateTime.Now,
                ReaderId = tyrellReader.Id
            };

            var like03 = new Like
            {
                CommentId = mrRobotPostComment01.Id,
                CreatedAt = DateTime.Now,
                ReaderId = angelaReader.Id
            };

            var like04 = new Like
            {
                CommentId = mrRobotPostComment02.Id,
                CreatedAt = DateTime.Now,
                ReaderId = tyrellReader.Id
            };

            var like05 = new Like
            {
                CommentId = linuxPostComment01.Id,
                CreatedAt = DateTime.Now,
                BloggerId = samBlogger.Id
            };

            var like06 = new Like
            {
                CommentId = linuxPostComment02.Id,
                CreatedAt = DateTime.Now,
                BloggerId = elliotBlogger.Id
            };

            await _context.Likes.AddRangeAsync(like01, like02, like03, like04, like05, like06);
            await _context.SaveChangesAsync();

            #endregion

            return Ok("Seed completed!");
        }
    }
}
