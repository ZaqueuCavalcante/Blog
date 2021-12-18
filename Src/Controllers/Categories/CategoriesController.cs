using Blog.Database;
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
        /// Returns a category.
        /// </summary>
        [HttpGet("{name}")]
        [AllowAnonymous]
        public async Task<ActionResult<CategoryOut>> GetCategory(string name)
        {
            var category = await _context.Categories
                .Include(c => c.Posts)
                .FirstOrDefaultAsync(c => c.Name == name);

            if (category is null)
                return NotFound("Category not found.");

            category.Posts = category.Posts.OrderByDescending(p => p.CreatedAt).ToList();

            return Ok(CategoryOut.New(category));
        }

        /// <summary>
        /// Returns all categories.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<CategoryOut>>> GetCategories()
        {
            var categories = await _context.Categories
                .Include(c => c.Posts)
                .ToListAsync();

            categories.ForEach(c => c.Posts = c.Posts.OrderByDescending(p => p.CreatedAt).ToList());

            return Ok(categories.Select(c => CategoryOut.New(c)).ToList());
        }
    }
}
