using Blog.Database;
using Blog.Domain;
using Blog.Extensions;
using Blog.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Blog.Configurations.AuthorizationConfigurations;

namespace Blog.Controllers.Readers;

[ApiController, Route("[controller]")]
public class ReadersController : ControllerBase
{
    private readonly BlogContext _context;
    private readonly UserManager<BlogUser> _userManager;

    public ReadersController(
        BlogContext context,
        UserManager<BlogUser> userManager
    ) {
        _context = context;
        _userManager = userManager;
    }

    /// <summary>
    /// Register a new reader.
    /// </summary>
    [HttpPost, AllowAnonymous]
    public async Task<IActionResult> PostReader(ReaderIn dto)
    {
        var user = new BlogUser(dto.Email);

        var reader = new Reader(dto.Name, user.Id);

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        await _userManager.AddToRoleAsync(user, ReaderRole);

        reader.UserId = user.Id;

        _context.Readers.Add(reader);
        await _context.SaveChangesAsync();

        return Created($"/readers/{reader.Id}", new ReaderOut(reader));
    }

    /// <summary>
    /// Returns a reader.
    /// </summary>
    [HttpGet("{id}"), AllowAnonymous]
    public async Task<ActionResult<ReaderOut>> GetReader(int id)
    {
        var reader = await _context.Readers
            .FirstOrDefaultAsync(l => l.Id == id);

        if (reader is null)
            return NotFound("Reader not found.");

        return Ok(new ReaderOut(reader));
    }

    /// <summary>
    /// Returns some readers.
    /// </summary>
    [HttpGet, AllowAnonymous]
    public async Task<ActionResult<List<ReaderOut>>> GetReaders([FromQuery] ReadersParameters parameters)
    {
        var readers = await _context.Readers
            .AsNoTracking()
            .Page(parameters)
            .ToListAsync();

        var count = await _context.Readers.CountAsync();

        Response.AddPagination(parameters, count);

        return Ok(readers.Select(r => new ReaderOut(r)).ToList());
    }
}
