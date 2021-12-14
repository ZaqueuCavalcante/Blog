using Blog.Database;
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
        /// Returns a category.
        /// </summary>
        [HttpGet("{name}")]
        public async Task<ActionResult<CategoryOut>> GetCategory(string name)
        {
            var category = await _context.Categories
                .Include(c => c.Posts)
                .FirstOrDefaultAsync(c => c.Name == name);

            if (category is null)
                return NotFound("Category not found.");

            return Ok(new CategoryOut(category));
        }

        /// <summary>
        /// Returns all the categories.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<CategoryOut>>> GetCategories()
        {
            var categories = await _context.Categories
                .Include(c => c.Posts)
                .ToListAsync();

            return Ok(categories.Select(c => new CategoryOut(c)).ToList());
        }
    }
}
