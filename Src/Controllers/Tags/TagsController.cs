using Blog.Database;
using Blog.Domain;
using Blog.Extensions;
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
        /// Register a new tag.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostTag(string name)
        {
            var tag = new Tag
            {
                Name = name,
                CreatedAt = DateTime.Now
            };

            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();

            return Created($"/tags/{tag.Id}", TagOut.New(tag, Request.GetRoot()));
        }

        /// <summary>
        /// Returns a tag.
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetTag(int id)
        {
            var tag = await _context.Tags
                .Include(t => t.Posts)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tag is null)
                return NotFound("Tag not found.");

            tag.Posts = tag.Posts.OrderByDescending(p => p.CreatedAt).ToList();

            return Ok(TagOut.New(tag, Request.GetRoot()));
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

            return Ok(tags.Select(t => TagOut.New(t, Request.GetRoot())).ToList());
        }
    }
}
