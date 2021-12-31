using Blog.Database;
using Blog.Domain;
using Blog.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers.Networks
{
    [ApiController, Route("[controller]")]
    public class NetworksController : ControllerBase
    {
        private readonly BlogContext _context;

        public NetworksController(
            BlogContext context
        ) {
            _context = context;
        }

        /// <summary>
        /// Add a new network.
        /// </summary>
        [HttpPost("networks"), Authorize]
        public async Task<ActionResult> PostNetwork([FromQuery] NetworkIn dto)  // TODO: refactor to FromBody?
        {
            var userId = User.GetId();

            var network = await _context.Networks.FirstOrDefaultAsync(
                n => n.UserId == userId && n.Name == dto.Name
            );

            if (network != null)
            {
                network.SetUri(dto.Uri);
                await _context.SaveChangesAsync();
                return Ok();
            }

            network = new Network(userId, dto.Name, dto.Uri);

            await _context.Networks.AddAsync(network);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
