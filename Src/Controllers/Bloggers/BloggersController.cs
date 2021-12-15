using Blog.Database;
using Blog.Domain;
using Blog.Identity;
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

        public BloggersController(
            BlogContext context,
            UserManager<User> userManager
        ) {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Register a new blogger.
        /// </summary>
        /// <returns>The registered blogger.</returns>
        [HttpPost]
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

            return Created($"/bloggers/{blogger.Id}", new BloggerOut(blogger));
        }

        /// <summary>
        /// Returns a blogger.
        /// </summary>
        /// <param name="id">The id of blogger.</param>
        /// <returns>A blogger.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<BloggerOut>> GetBlogger(int id)
        {
            var blogger = await _context.Bloggers
                .Include(b => b.Networks)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (blogger is null)
                return NotFound("Blogger not found.");

            return Ok(new BloggerOut(blogger));
        }

        /// <summary>
        /// Returns all the bloggers.
        /// </summary>
        /// <returns>A list of bloggers.</returns>
        [HttpGet]
        public async Task<ActionResult<List<BloggerOut>>> GetBloggers()
        {
            var bloggers = await _context.Bloggers
                .ToListAsync();

            return Ok(bloggers.Select(x => new BloggerOut(x)).ToList());
        }

        /// <summary>
        /// Update a blogger.
        /// </summary>
        [HttpPut("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
        public async Task<IActionResult> UpdateBlogger(int id, BloggerUpdateIn dto)
        {
            var blogger = await _context.Bloggers.FirstOrDefaultAsync(b => b.Id == id);

            if (blogger == null)
                return NotFound("Blogger not found.");

            blogger.Update(dto.Name, dto.Resume);

            _context.Bloggers.Update(blogger);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Delete a blogger.
        /// </summary>
        [HttpDelete("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
        public async Task<IActionResult> DeleteBlogger(int id)
        {
            var blogger = await _context.Bloggers.FirstOrDefaultAsync(b => b.Id == id);

            if (blogger == null)
                return NotFound("Blogger not found.");

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == blogger.UserId);
            await _userManager.DeleteAsync(user);

            return Ok();
        }
    }
}
