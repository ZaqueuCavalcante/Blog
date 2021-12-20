using Blog.Extensions;
using Blog.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blog.Controllers.Bloggers;

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

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> GetHome()
        {
            var url = Request.GetRoot();

            var lastPosts = await _context.Posts
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
            var lastPostsOut = lastPosts.Select(p => new {
                Id = p.Id,
                Title = p.Title,
                Date = p.CreatedAt.Format(),
                Resume = p.Resume
            }).ToList();

            var bloggers = await _context.Bloggers
                .ToListAsync();
            var bloggersOut = new List<BloggerOut>();
            foreach (var blogger in bloggers)
            {
                var networks = await _context.Networks.Where(n => n.UserId == blogger.UserId).ToListAsync();
                var bloggerOut = BloggerOut.New(blogger, networks);
                bloggerOut.Link = url + "bloggers/" + blogger.Id;
                bloggersOut.Add(bloggerOut);
            }

            var categories = await _context.Categories
                .Include(c => c.Posts)
                .ToListAsync();
            var categoriesOut = categories.Select(c => new {
                Id = c.Id,
                Name = c.Name,
                Posts = c.Posts.Count
            });

            var tags = await _context.Tags
                .ToListAsync();
            var tagsOut = tags.Select(t => new {
                Id = t.Id,
                Name = t.Name
            });

            var response = new
            {
                Links = new
                {
                    Home = url,
                    Bloggers = url + "bloggers",
                    Posts = url + "posts",
                    Categories = url + "categories",
                    Tags = url + "tags"
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
