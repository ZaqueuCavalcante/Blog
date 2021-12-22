using Blog.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers.Search
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly BlogContext _context;

        public SearchController(
            BlogContext context
        ) {
            _context = context;
        }

        [HttpGet("{thing}")]
        [AllowAnonymous]
        public async Task<ActionResult> Search(string thing)
        {
            var bloggers = await _context.Bloggers
                .Where(b => b.Name == thing).ToListAsync();
            
            // Posts (Title, Resume, Body)
            // Categories
            // Tags

            // Return object with all matches?
            // Make with Dapper?
            // Full Text Search PostgreSQL

            return Ok(bloggers);
        }
    }
}
