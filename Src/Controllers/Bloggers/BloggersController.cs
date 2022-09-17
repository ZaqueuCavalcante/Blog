using Blog.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Blog.Configurations.AuthorizationConfigurations;
using Blog.Services.Bloggers;

namespace Blog.Controllers.Bloggers;

[ApiController, Route("[controller]")]
public class BloggersController : ControllerBase
{
    private readonly IBloggersService _bloggersService;

    public BloggersController(
        IBloggersService bloggersService
    ) {
        _bloggersService = bloggersService;
    }

    /// <summary>
    /// Register a new blogger.
    /// </summary>
    [HttpPost, Authorize(Roles = AdminRole)]
    [ProducesResponseType(typeof(BloggerOut), 201)]
    public async Task<IActionResult> PostBlogger(BloggerIn data)
    {
        var blogger = await _bloggersService.CreateBlogger(
            data.Name, data.Resume, data.Email, data.Password
        );

        return Created($"/bloggers/{blogger.Id}", new BloggerOut(blogger));
    }

    /// <summary>
    /// Returns a blogger.
    /// </summary>
    [HttpGet("{id}"), AllowAnonymous]
    [ProducesResponseType(typeof(BloggerOut), 200)]
    public async Task<ActionResult> GetBlogger(int id)
    {
        var blogger = await _bloggersService.GetBlogger(id);

        return Ok(new BloggerOut(blogger));
    }

    /// <summary>
    /// Returns all the bloggers.
    /// </summary>
    [HttpGet, AllowAnonymous]
    [ProducesResponseType(typeof(List<BloggerOut>), 200)]
    public async Task<ActionResult> GetBloggers()
    {
        var bloggers = await _bloggersService.GetBloggers();

        return Ok(bloggers.Select(b => new BloggerOut(b)).ToList());
    }

    /// <summary>
    /// Update a blogger.
    /// </summary>
    [HttpPatch, Authorize(Roles = BloggerRole)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> UpdateBlogger(BloggerUpdateIn data)
    {
        await _bloggersService.UpdateBlogger(User.Id(), data.Name, data.Resume);

        return NoContent();
    }
}
