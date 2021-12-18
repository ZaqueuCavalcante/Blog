using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Blog.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers.Users
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private IConfiguration _configuration { get; }

        public UsersController(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            SignInManager<User> signInManager,
            IConfiguration configuration
        ) {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(UserIn dto)
        {
            var result = await _signInManager.PasswordSignInAsync(
                userName: dto.Email,
                password: dto.Password,
                isPersistent: true,
                lockoutOnFailure: true
            );

            if (result.Succeeded)
            {
                var response = new LoginOut
                {
                    AccessToken = await GetJwt(dto.Email),
                    TokenType = "Bearer",
                    ExpiresIn = _configuration["Jwt:ExpirationTimeInSeconds"],
                    RefreshToken = "",
                    Scope = "create"
                };

                return Ok(response);
            }

            if (result.IsLockedOut)
                return Ok("Account Locked");

            if (result.IsNotAllowed)
                return Ok("Login Not Allowed");

            if (result.RequiresTwoFactor)
                return Ok("Requires Two Factor");
            
            return Ok("Login Failed");
        }

        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            
            return Ok("Logout succeeded");
        }

        [HttpPut("change-password")]
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

            var claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));

            var roleNames = await _userManager.GetRolesAsync(user);
            foreach (var role in roleNames)
            {
                claims.Add(new Claim("role", role));
            }

            var roles = await _roleManager.Roles.Where(r => roleNames.Contains(r.Name)).ToListAsync();
            if (roles != null && roles.Any())
            {
                foreach (var role in roles)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    if (roleClaims != null && roleClaims.Any())
                        claims.AddRange(roleClaims);
                }
            }

            var userClaims = await _userManager.GetClaimsAsync(user);
            if (userClaims != null && userClaims.Any())
                claims.AddRange(userClaims);

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
    }
}
