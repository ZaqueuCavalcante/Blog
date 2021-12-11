using Blog.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Blog.Controllers.Identity
{
    [ApiController]
    [Route("[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public IdentityController(
            UserManager<User> userManager,
            SignInManager<User> signInManager
        ) {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("users")]
        public async Task<ActionResult> PostUser(UserIn dto)
        {
            var user = new User
            {
                UserName = dto.Name,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
                return Created($"identity/users/{user.Id}", user);

            return BadRequest(result.Errors);
        }

        [HttpPost("users/login")]
        public async Task<ActionResult> Login(UserIn dto)
        {
            SignInResult result = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, true, true);

            if (result.Succeeded)
                return Ok("Succeeded");
            
            if (result.IsLockedOut)
                return Ok("Account Locked");

            if (result.IsNotAllowed)
                return Ok("Sign In Not Allowed");

            if (result.RequiresTwoFactor)
                return Ok("Requires Two Factor");

            if (result.IsLockedOut)
                return Ok("Requires Two Factor");
            
            return Ok("Sign In Failed");
        }




        [HttpGet("features")]
        public async Task<ActionResult> GetFeatures()
        {
            var features = _userManager.GetType().GetProperties()
                .Where(prop => prop.Name.StartsWith("Supports"))
                .OrderBy(p => p.Name)
                .Select(prop => (prop.Name, prop.GetValue(_userManager)
                .ToString())).ToList();

            var result = features.Select(t => new { Feature = t.Item1, Supports = t.Item2 });

            return Ok(result);
        }
    }
}
