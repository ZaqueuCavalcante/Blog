using Blog.Database;
using Blog.Domain;
using Blog.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers.Bloggers
{
    [ApiController]
    [Route("[controller]")]
    public class BloggersController : ControllerBase
    {
        private readonly BlogContext _context;

        public BloggersController(BlogContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> PostBlogger(BloggerIn dto)
        {
            var blogger = await _context.Bloggers.FirstOrDefaultAsync(b => b.Name.ToLower() == dto.Name.ToLower());
            if (blogger != null)
                throw new DomainException("A blogger with this name already exists.");

            blogger = new Blogger
            {
                Name = dto.Name,
                Resume = dto.Resume,
                CreatedAt = DateTime.Now
            };

            _context.Bloggers.Add(blogger);
            await _context.SaveChangesAsync();

            return Created($"/bloggers/{blogger.Id}", new BloggerOut(blogger));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BloggerOut>> GetBlogger(int id)
        {
            var blogger = await _context.Bloggers
                .FirstOrDefaultAsync(l => l.Id == id);

            if (blogger is null)
                return NotFound("Blogger not found.");

            return Ok(new BloggerOut(blogger));
        }

        [HttpGet]
        public async Task<ActionResult<List<BloggerOut>>> GetBloggers()
        {
            var bloggers = await _context.Bloggers
                .ToListAsync();

            return Ok(bloggers.Select(x => new BloggerOut(x)).ToList());
        }
    }
}
