using Blog.Database;
using Blog.Domain;
using Blog.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Blog.Configurations.AuthorizationConfigurations;

namespace Blog.Controllers.Tags
{
    [ApiController, Route("[controller]")]
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
        [HttpPost, Authorize(Roles = AdminRole)]
        public async Task<IActionResult> PostTag(TagIn dto)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(
                t => t.Name.ToLower() == dto.Name.Trim().ToLower());

            if (tag != null)
                return BadRequest("Tag already exists.");

            tag = new Tag(dto.Name);

            await _context.Tags.AddAsync(tag);
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
        /// Returns some tags.
        /// </summary>
        [HttpGet, AllowAnonymous]
        public async Task<ActionResult<List<TagOut>>> GetTags([FromQuery] TagsParameters parameters)
        {
            var tags = await _context.Tags.AsNoTracking()
                .Include(t => t.Posts.OrderByDescending(p => p.CreatedAt))
                .OrderBy(t => t.Name)
                .Page(parameters)
                .ToListAsync();

            var count = await _context.Tags.CountAsync();

            Response.AddPagination(parameters, count);

            return Ok(tags.Select(t => TagOut.New(t, Request.GetRoot())).ToList());
        }

        /// <summary>
        /// Delete a tag.
        /// </summary>
        [HttpDelete("{id}"), Authorize(Roles = AdminRole)]
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
