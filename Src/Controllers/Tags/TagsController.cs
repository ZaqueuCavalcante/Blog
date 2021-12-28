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
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostTag(string name)
        {
            var tag = new Tag(name);

            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();

            return Created($"/tags/{tag.Id}", TagOut.New(tag, Request.GetRoot()));
        }

        /// <summary>
        /// Returns a tag.
        /// </summary>
        [HttpGet("{id}"), AllowAnonymous]
        public async Task<ActionResult> GetTag(int id)
        {
            var tag = await _context.Tags
                .Include(t => t.Posts.OrderByDescending(p => p.CreatedAt))
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tag is null)
                return NotFound("Tag not found.");

            return Ok(TagOut.New(tag, Request.GetRoot()));
        }

        /// <summary>
        /// Returns all the tags.
        /// </summary>
        [HttpGet, AllowAnonymous]
        public async Task<ActionResult<List<TagOut>>> GetTags()
        {
            var tags = await _context.Tags
                .Include(t => t.Posts.OrderByDescending(p => p.CreatedAt))
                .ToListAsync();

            return Ok(tags.Select(t => TagOut.New(t, Request.GetRoot())).ToList());
        }

        /// <summary>
        /// Delete a tag.
        /// </summary>
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteTag(int id)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id);

            if (tag is null)
                return NotFound("Tag not found.");

            _context.Remove(tag);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
