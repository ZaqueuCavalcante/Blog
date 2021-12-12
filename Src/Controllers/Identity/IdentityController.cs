using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Blog.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Blog.Controllers.Identity
{
    [ApiController]
    [Route("[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private IConfiguration _configuration { get; }

        public IdentityController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration
        ) {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
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
            var result = await _signInManager.PasswordSignInAsync(
                userName: dto.Email,
                password: dto.Password,
                isPersistent: true,
                lockoutOnFailure: true
            );

            if (result.Succeeded)
                return Ok(new { Jwt = await GetJwt(dto.Email) });

            if (result.IsLockedOut)
                return Ok("Account Locked");

            if (result.IsNotAllowed)
                return Ok("Login Not Allowed");

            if (result.RequiresTwoFactor)
                return Ok("Requires Two Factor");
            
            return Ok("Login Failed");
        }

        [HttpPost("users/logout")]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            
            return Ok("Logout succeeded");
        }

        [HttpPost("users/change-password")]
        public async Task<ActionResult> ChangePassword(ChangePasswordIn dto)
        {
            var user = await _userManager.GetUserAsync(User);

            var result = await _userManager.ChangePasswordAsync(
                user,
                dto.Current,
                dto.NewPassword
            );

            if (result.Succeeded)
                return Ok("Password changed.");

            return Ok("Password not changed.");
        }

        private async Task<string> GetJwt(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var claims = await _userManager.GetClaimsAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecurityKey"]);
            var expirationTime = double.Parse(_configuration["Jwt:ExpirationTimeInSeconds"]);
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                Expires = DateTime.UtcNow.AddSeconds(expirationTime),
                SigningCredentials = signingCredentials,
                Subject = identityClaims
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encodedToken = tokenHandler.WriteToken(token);

            return encodedToken;
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
