using Blog.Database;
using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
        public async Task<ActionResult> GetTag(string name)
        {
            var tag = await _context.Tags
                .Include(c => c.Posts)
                .FirstOrDefaultAsync(c => c.Name == name);

            if (tag is null)
                return NotFound("Tag not found.");

            tag.Posts = tag.Posts.OrderByDescending(p => p.CreatedAt).ToList();

            return Ok(TagOut.New(tag));
        }

        /// <summary>
        /// Returns all the tags.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<TagOut>>> GetTags()
        {
            var tags = await _context.Tags
                .Include(c => c.Posts)
                .ToListAsync();

            tags.ForEach(t => t.Posts = t.Posts.OrderByDescending(p => p.CreatedAt).ToList());

            return Ok(tags.Select(t => TagOut.New(t)).ToList());
        }
    }
}
