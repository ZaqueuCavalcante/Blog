using Blog.Database;
using Blog.Domain;
using Blog.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Blog.Configurations.ControllersConfigurations;
using static Blog.Configurations.AuthorizationConfigurations;

namespace Blog.Controllers.Categories
{
    [ApiController, Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly BlogContext _context;

        public CategoriesController(
            BlogContext context
        ) {
            _context = context;
        }

        /// <summary>
        /// Register a new category.
        /// </summary>
        [HttpPost, Authorize(Roles = AdminRole)]
        public async Task<IActionResult> PostCategory(CategoryIn dto)
        {
            var category = new Category(dto.Name, dto.Description);

            var categoryAlreadyExists = await _context.Categories.AnyAsync(
                c => c.Name.ToLower() == dto.Name.Trim().ToLower());

            if (categoryAlreadyExists)
                return BadRequest("Category already exists.");

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return Created($"/categories/{category.Id}", CategoryOut.New(category, Request.GetRoot()));
        }

        /// <summary>
        /// Returns a category.
        /// </summary>
        [HttpGet("{id}"), AllowAnonymous]
        public async Task<ActionResult<CategoryOut>> GetCategory(int id)
        {
            var category = await _context.Categories.AsNoTracking()
                .Include(c => c.Posts.OrderByDescending(p => p.CreatedAt))
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category is null)
                return NotFound("Category not found.");

            return Ok(CategoryOut.New(category, Request.GetRoot()));
        }

        /// <summary>
        /// Returns all the categories.
        /// </summary>
        [HttpGet, AllowAnonymous]
        [ResponseCache(CacheProfileName = TwoMinutesCacheProfile)]
        public async Task<ActionResult<List<CategoryOut>>> GetCategories([FromQuery] CategoryParameters parameters)
        {
            var categories = await _context.Categories.AsNoTracking()
                .Include(c => c.Posts.OrderByDescending(p => p.CreatedAt))
                .OrderBy(c => c.Name)
                .Page(parameters)
                .ToListAsync();

            var count = await _context.Categories.CountAsync();

            Response.AddPagination(parameters, count);

            return Ok(categories.Select(c => CategoryOut.New(c, Request.GetRoot())).ToList());
        }
    }
}
