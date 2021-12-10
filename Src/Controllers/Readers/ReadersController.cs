using Blog.Database;
using Blog.Domain;
using Blog.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers.Readers
{
    [ApiController]
    [Route("[controller]")]
    public class ReadersController : ControllerBase
    {
        private readonly BlogContext _context;

        public ReadersController(BlogContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> PostReader(ReaderIn dto)
        {
            var reader = await _context.Readers.FirstOrDefaultAsync(b => b.Name.ToLower() == dto.Name.ToLower());
            if (reader != null)
                throw new DomainException("A reader with this name already exists.");

            reader = new Reader
            {
                Name = dto.Name,
                CreatedAt = DateTime.Now
            };

            _context.Readers.Add(reader);
            await _context.SaveChangesAsync();

            return Created($"/readers/{reader.Id}", new ReaderOut(reader));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReaderOut>> GetReader(int id)
        {
            var user = HttpContext.User;

            var claims = user.Claims;


            var reader = await _context.Readers
                .FirstOrDefaultAsync(l => l.Id == id);

            if (reader is null)
                return NotFound("Reader not found.");

            return Ok(new ReaderOut(reader));
        }

        [HttpGet]
        public async Task<ActionResult<List<ReaderOut>>> GetReaders()
        {
            var readers = await _context.Readers
                .ToListAsync();

            return Ok(readers.Select(x => new ReaderOut(x)).ToList());
        }
    }
}
