using Blog.Database;
using Blog.Domain;
using Blog.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers.Categories
{
    [ApiController]
    [Route("[controller]")]
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
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostCategory(CategoryIn dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description,
                CreatedAt = DateTime.Now
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return Created($"/categories/{category.Id}", CategoryOut.New(category, Request.GetRoot()));
        }

        /// <summary>
        /// Returns a category.
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<CategoryOut>> GetCategory(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Posts)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category is null)
                return NotFound("Category not found.");

            category.Posts = category.Posts.OrderByDescending(p => p.CreatedAt).ToList();

            return Ok(CategoryOut.New(category, Request.GetRoot()));
        }

        /// <summary>
        /// Returns all the categories.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<CategoryOut>>> GetCategories()
        {
            var categories = await _context.Categories
                .Include(c => c.Posts)
                .ToListAsync();

            categories.ForEach(c => c.Posts = c.Posts.OrderByDescending(p => p.CreatedAt).ToList());

            return Ok(categories.Select(c => CategoryOut.New(c, Request.GetRoot())).ToList());
        }
    }
}
