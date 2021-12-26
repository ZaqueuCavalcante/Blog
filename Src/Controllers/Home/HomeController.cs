using Blog.Extensions;
using Blog.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers.Home
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        private readonly BlogContext _context;

        public HomeController(
            BlogContext context
        ) {
            _context = context;
        }

        /// <summary>
        /// Returns all informations about the blog home page.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> GetHome()
        {
            var url = Request.GetRoot();

            var lastPosts = await _context.Posts
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
            var lastPostsOut = lastPosts.Select(p => HomePostOut.New(p, url)).ToList();
 
            var bloggers = await _context.Bloggers
                .ToListAsync();
            var bloggersOut = new List<HomeBloggerOut>();
            foreach (var blogger in bloggers)
            {
                var networks = await _context.Networks.Where(n => n.UserId == blogger.UserId).ToListAsync();
                bloggersOut.Add(HomeBloggerOut.New(blogger, networks, url));
            }

            var categories = await _context.Categories
                .Include(c => c.Posts)
                .ToListAsync();
            var categoriesOut = categories.Select(c => HomeCategoryOut.New(c, url));

            var tags = await _context.Tags
                .ToListAsync();
            var tagsOut = tags.Select(t => HomeTagOut.New(t, url));

            var response = new
            {
                Links = new
                {
                    Home = url,
                    Search = url + "search",
                    Bloggers = url + "bloggers",
                    Posts = url + "posts",
                    Categories = url + "categories",
                    Tags = url + "tags",
                    Login = url + "users/login"
                },
                LastPosts = lastPostsOut,
                Bloggers = bloggersOut,
                Categories = categoriesOut,
                Tags = tagsOut
            };

            return Ok(response);
        }
    }
}
