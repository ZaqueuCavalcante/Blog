using Blog.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers.Tags
{
    [ApiController]
    [Route("[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly BlogContext _context;

        public TagsController(
            BlogContext context
        ) {
            _context = context;
        }

        /// <summary>
        /// Returns a tag.
        /// </summary>
        [HttpGet("{name}")]
        public async Task<ActionResult<TagOut>> GetTag(string name)
        {
            var tag = await _context.Tags
                .Include(c => c.Posts)
                .FirstOrDefaultAsync(c => c.Name == name);

            if (tag is null)
                return NotFound("Tag not found.");

            return Ok(new TagOut(tag));
        }

        /// <summary>
        /// Returns all the tags.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<TagOut>>> GetTags()
        {
            var tags = await _context.Tags
                .Include(c => c.Posts)
                .ToListAsync();

            return Ok(tags.Select(t => new TagOut(t)).ToList());
        }
    }
}
