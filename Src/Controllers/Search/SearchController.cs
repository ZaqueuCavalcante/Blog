using Blog.Extensions;
using Blog.Settings;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace Blog.Controllers.Search;

[ApiController, Route("[controller]")]
public class SearchController : ControllerBase
{
    public SearchController() {}

    /// <summary>
    /// Search for something on the blog.
    /// </summary>
    [HttpGet, AllowAnonymous]
    public async Task<ActionResult> Search([FromQuery] SearchParameters parameters, [FromServices] DatabaseSettings dbSettings)
    {
        // TODO: https://lucenenet.apache.org/index.html
        var url = Request.GetRoot();

        var bloggersSql = @"
            SELECT id, name FROM blog.bloggers
            WHERE name ILIKE '%' || @Thing || '%' OR resume ILIKE '%' || @Thing || '%'
            ORDER BY name
        ";

        var categoriesSql = @"
            SELECT id, name FROM blog.categories
            WHERE name ILIKE '%' || @Thing || '%' OR description ILIKE '%' || @Thing || '%'
            ORDER BY name
        ";

        var postsSql = @"
            SELECT id, title AS name FROM blog.posts
            WHERE title ILIKE '%' || @Thing || '%' OR resume ILIKE '%' || @Thing || '%' OR body ILIKE '%' || @Thing || '%'
            ORDER BY created_at DESC
        ";

        var tagsSql = @"
            SELECT id, name FROM blog.tags
            WHERE name ILIKE '%' || @Thing || '%'
            ORDER BY name
        ";

        using (var connection = new NpgsqlConnection(dbSettings.ConnectionString))
        {
            var bloggers = (await connection.QueryAsync<SearchOut>(bloggersSql, new { parameters.Thing })).ToList();
            var categories = (await connection.QueryAsync<SearchOut>(categoriesSql, new { parameters.Thing })).ToList();
            var posts = (await connection.QueryAsync<SearchOut>(postsSql, new { parameters.Thing })).ToList();
            var tags = (await connection.QueryAsync<SearchOut>(tagsSql, new { parameters.Thing })).ToList();

            bloggers.ForEach(b => b.Link = url + "bloggers/" + b.Id);
            categories.ForEach(c => c.Link = url + "categories/" + c.Id);
            posts.ForEach(p => { p.Link = url + "posts/" + p.Id; });
            tags.ForEach(t => { t.Link = url + "tags/" + t.Id; });

            return Ok(new { bloggers, categories, posts, tags });
        }
    }
}
