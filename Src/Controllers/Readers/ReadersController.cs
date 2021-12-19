using Blog.Database;
using Blog.Domain;
using Blog.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers.Readers
{
    [ApiController]
    [Route("[controller]")]
    public class ReadersController : ControllerBase
    {
        private readonly BlogContext _context;
        private readonly UserManager<User> _userManager;

        public ReadersController(
            BlogContext context,
            UserManager<User> userManager
        ) {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Register a new reader.
        /// </summary>
        /// <returns>The registered reader.</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PostReader(ReaderIn dto)
        {
            var user = new User
            {
                UserName = dto.Email,
                Email = dto.Email
            };

            var reader = new Reader(dto.Name, user.Id);

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            reader.UserId = user.Id;

            _context.Readers.Add(reader);
            await _context.SaveChangesAsync();

            return Created($"/readers/{reader.Id}", ReaderOut.New(reader));
        }

        /// <summary>
        /// Returns a reader, given your id.
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ReaderOut>> GetReader(int id)
        {
            var reader = await _context.Readers
                .FirstOrDefaultAsync(l => l.Id == id);

            if (reader is null)
                return NotFound("Reader not found.");

            return Ok(ReaderOut.New(reader));
        }

        /// <summary>
        /// Returns all the readers.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<ReaderOut>>> GetReaders()
        {
            var readers = await _context.Readers
                .ToListAsync();

            return Ok(readers.Select(r => ReaderOut.New(r)).ToList());
        }
    }
}
