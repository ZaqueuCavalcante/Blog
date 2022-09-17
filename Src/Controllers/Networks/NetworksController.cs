using Blog.Database;
using Blog.Domain;
using Blog.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Blog.Configurations.AuthorizationConfigurations;

namespace Blog.Controllers.Networks;

[ApiController, Route("[controller]"), Authorize(Roles = BloggerRole)]
public class NetworksController : ControllerBase
{
    private readonly BlogContext _context;

    public NetworksController(
        BlogContext context
    ) {
        _context = context;
    }

    /// <summary>
    /// Add or update a network.
    /// </summary>
    [HttpPost("networks")]
    public async Task<ActionResult> PostNetwork([FromQuery] NetworkIn dto)
    {
        var bloggerId = await _context.Bloggers.Where(b => b.UserId == User.Id())
            .Select(b => b.Id).FirstAsync();

        var network = await _context.Networks.FirstOrDefaultAsync(
            n => n.BloggerId == bloggerId && n.Name == dto.Name
        );

        if (network != null)
        {
            network.SetUri(dto.Uri);
            await _context.SaveChangesAsync();
            return Ok();
        }

        network = new Network(bloggerId, dto.Name, dto.Uri);

        await _context.Networks.AddAsync(network);
        await _context.SaveChangesAsync();

        return Ok();
    }

    /// <summary>
    /// Delete a network.
    /// </summary>
    [HttpDelete("networks")]
    public async Task<ActionResult> DeleteNetwork([FromQuery] DeleteNetworkIn dto)
    {
        var bloggerId = await _context.Bloggers.Where(b => b.UserId == User.Id())
            .Select(b => b.Id).FirstAsync();

        var network = await _context.Networks.FirstOrDefaultAsync(
            n => n.BloggerId == bloggerId && n.Name == dto.Name
        );

        if (network == null)
            return NotFound("Network not found.");

        _context.Networks.Remove(network);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
