using System.Security.Claims;
using Blog.Database;
using Blog.Domain;
using Blog.Extensions;
using Blog.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers.Bloggers
{
    [ApiController]
    [Route("[controller]")]
    public class BloggersController : ControllerBase
    {
        private readonly BlogContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _env;

        public BloggersController(
            BlogContext context,
            UserManager<User> userManager,
            IWebHostEnvironment env
        ) {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        /// <summary>
        /// Register a new blogger.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostBlogger(BloggerIn dto)
        {
            var user = new User
            {
                UserName = dto.Email,
                Email = dto.Email
            };

            var blogger = new Blogger(dto.Name, dto.Resume, user.Id);

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            blogger.UserId = user.Id;

            _context.Bloggers.Add(blogger);
            await _context.SaveChangesAsync();

            return Created($"/bloggers/{blogger.Id}", BloggerOut.New(blogger));
        }

        /// <summary>
        /// Returns a blogger.
        /// </summary>
        /// <param name="id">The id of blogger.</param>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<BloggerOut>> GetBlogger(int id)
        {
            var blogger = await _context.Bloggers
                .Include(b => b.Posts)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (blogger is null)
                return NotFound("Blogger not found.");

            var networks = await _context.Networks.Where(n => n.UserId == blogger.UserId).ToListAsync();

            return Ok(BloggerOut.New(blogger, networks, Request.GetRoot()));
        }

        /// <summary>
        /// Returns all the bloggers.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<BloggerOut>>> GetBloggers()
        {
            var bloggers = await _context.Bloggers
                .ToListAsync();

            return Ok(bloggers.Select(x => BloggerOut.New(x)).ToList());
        }

        /// <summary>
        /// Update a blogger.
        /// </summary>
        [HttpPut]
        [Authorize(Roles = "Blogger")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
        public async Task<IActionResult> UpdateBlogger(BloggerUpdateIn dto)
        {
            var userId = int.Parse(User.FindFirstValue("sub"));

            var blogger = await _context.Bloggers.FirstOrDefaultAsync(b => b.UserId == userId);

            blogger.Update(dto.Name, dto.Resume);

            _context.Bloggers.Update(blogger);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("stats")]
        [Authorize(Roles = "Blogger")]
        public async Task<ActionResult> GetStats()
        {
            var userId = int.Parse(User.FindFirstValue("sub"));
            var blogger = await _context.Bloggers.FirstAsync(b => b.UserId == userId);

            var publishedPosts = await _context.Posts
                .Where(p => p.Authors.Contains(blogger))
                .Select(p => p.Id)
                .ToListAsync();
            var draftPosts = 0;

            var latestComments = await _context.Comments
                .Where(c => publishedPosts.Contains(c.PostId))
                .OrderByDescending(c => c.CreatedAt)
                .Take(5)
                .ToListAsync();

            var response = BloggerStatsOut.New(
                publishedPosts.Count,
                draftPosts,
                latestComments
            );

            return Ok(response);
        }
    }
}
