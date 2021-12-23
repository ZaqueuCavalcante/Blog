using Blog.Extensions;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Blog.Controllers.Search
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public SearchController(
            IConfiguration configuration
        ) {
            _configuration = configuration;
        }

        [HttpGet("{thing}")]
        [AllowAnonymous]
        public async Task<ActionResult> Search(string thing)
        {
            var url = Request.GetRoot();

            var bloggersSql = @"
                SELECT id, name FROM blog.bloggers
                WHERE name ILIKE '%' || @Thing || '%' OR resume ILIKE '%' || @Thing || '%'
            ";

            var categoriesSql = @"
                SELECT id, name FROM blog.categories
                WHERE name ILIKE '%' || @Thing || '%' OR description ILIKE '%' || @Thing || '%'
            ";

            var postsSql = @"
                SELECT id, title AS name FROM blog.posts
                WHERE title ILIKE '%' || @Thing || '%' OR resume ILIKE '%' || @Thing || '%' OR body ILIKE '%' || @Thing || '%'
                ORDER BY created_at DESC
            ";

            var tagsSql = @"
                SELECT id, name FROM blog.tags
                WHERE name ILIKE '%' || @Thing || '%'
            ";

            using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("Connection")))
            {
                var bloggers = (await connection.QueryAsync<SearchOut>(bloggersSql, new { Thing = thing })).ToList();
                var categories = (await connection.QueryAsync<SearchOut>(categoriesSql, new { Thing = thing })).ToList();
                var posts = (await connection.QueryAsync<SearchOut>(postsSql, new { Thing = thing })).ToList();
                var tags = (await connection.QueryAsync<SearchOut>(tagsSql, new { Thing = thing })).ToList();

                bloggers.ForEach(b => b.Link = url + "bloggers/" + b.Id);
                categories.ForEach(c => c.Link = url + "categories/" + c.Id);
                posts.ForEach(p => { p.Link = url + "posts/" + p.Id; });
                tags.ForEach(t => { t.Link = url + "tags/" + t.Id; });

                return Ok(new { Bloggers = bloggers, Categories = categories, Posts = posts, Tags = tags });
            }
        }
    }
}
