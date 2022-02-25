using Blog.Database;
using Blog.Domain;
using Blog.Extensions;
using Blog.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Blog.Configurations.AuthorizationConfigurations;

namespace Blog.Controllers.Bloggers;

[ApiController, Route("[controller]")]
public class BloggersController : ControllerBase
{
    private readonly BlogContext _context;
    private readonly UserManager<BlogUser> _userManager;

    public BloggersController(
        BlogContext context,
        UserManager<BlogUser> userManager
    ) {
        _context = context;
        _userManager = userManager;
    }

    /// <summary>
    /// Register a new blogger.
    /// </summary>
    [HttpPost, Authorize(Roles = AdminRole)]
    public async Task<IActionResult> PostBlogger(BloggerIn dto)
    {
        var user = new BlogUser(dto.Email);

        var blogger = new Blogger(dto.Name, dto.Resume, user.Id);

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        await _userManager.AddToRoleAsync(user, BloggerRole);

        blogger.UserId = user.Id;

        _context.Bloggers.Add(blogger);
        await _context.SaveChangesAsync();

        return Created($"/bloggers/{blogger.Id}", BloggerOut.New(blogger));
    }

    /// <summary>
    /// Returns a blogger.
    /// </summary>
    [HttpGet("{id}"), AllowAnonymous]
    public async Task<ActionResult<BloggerOut>> GetBlogger(int id)
    {
        var blogger = await _context.Bloggers.AsNoTracking()
            .Include(b => b.Posts)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (blogger is null)
            return NotFound("Blogger not found.");

        var networks = await _context.Networks.Where(n => n.UserId == blogger.UserId).ToListAsync();

        return Ok(BloggerOut.New(blogger, networks, Request.GetRoot()));
    }

    /// <summary>
    /// Returns all the bloggers.
    /// </summary>
    [HttpGet, AllowAnonymous]
    public async Task<ActionResult<List<BloggerOut>>> GetBloggers()
    {
        var bloggers = await _context.Bloggers.AsNoTracking()
            .ToListAsync();

        return Ok(bloggers.Select(x => BloggerOut.New(x, null, Request.GetRoot())).ToList());
    }

    /// <summary>
    /// Update a blogger.
    /// </summary>
    [HttpPatch, Authorize(Roles = BloggerRole)]
    public async Task<IActionResult> UpdateBlogger(BloggerUpdateIn dto)
    {
        var blogger = await _context.Bloggers.FirstAsync(b => b.UserId == User.GetId());
        blogger.Update(dto.Name, dto.Resume);

        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Return statistics about the blogger.
    /// </summary>
    [HttpGet("stats"), Authorize(Roles = BloggerRole)]
    public async Task<ActionResult> GetStats()
    {
        var blogger = await _context.Bloggers.AsNoTracking()
            .FirstAsync(b => b.UserId == User.GetId());

        var publishedPosts = await _context.Posts.AsNoTracking()
            .Where(p => p.AuthorId == blogger.Id)
            .Select(p => p.Id)
            .ToListAsync();

        var draftPosts = 0;

        var latestComments = await _context.Comments.AsNoTracking()
            .Where(c => publishedPosts.Contains(c.PostId))
            .OrderByDescending(c => c.CreatedAt)
            .Take(5)
            .ToListAsync();

        var response = BloggerStatsOut.New(
            publishedPosts.Count,
            draftPosts,
            latestComments
        );

        return Ok(response);
    }
}
