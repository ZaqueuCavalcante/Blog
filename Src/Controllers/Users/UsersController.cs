using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Blog.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using Blog.Extensions;
using Blog.Database;
using Blog.Domain;

namespace Blog.Controllers.Users
{
    [ApiController, Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly BlogContext _context;
        private IConfiguration _configuration;
        private TokenValidationParameters _tokenValidationParameters;

        public UsersController(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            SignInManager<User> signInManager,
            BlogContext context,
            IConfiguration configuration
        ) {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
            _configuration = configuration;
            SetTokenValidationParameters();
        }

        /// <summary>
        /// Login into blog.
        /// </summary>
        [HttpPost("login"), AllowAnonymous]
        public async Task<ActionResult> Login(UserIn dto)
        {
            var result = await _signInManager.PasswordSignInAsync(
                userName: dto.Email,
                password: dto.Password,
                isPersistent: false,
                lockoutOnFailure: true
            );

            if (result.Succeeded)
            {
                var response = await GenerateLoginResponse(dto.Email);
                return Ok(response);
            }

            if (result.IsLockedOut)
                return Ok("Account locked.");

            if (result.IsNotAllowed)
                return Ok("Login not allowed.");

            if (result.RequiresTwoFactor)
                return Ok("Requires two factor.");
            
            return Ok("Login failed.");
        }

        /// <summary>
        /// Logout of the blog.
        /// </summary>
        [HttpPost("logout"), Authorize]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            
            return Ok("Logout succeeded.");
        }

        /// <summary>
        /// Change user password.
        /// </summary>
        [HttpPatch("change-password"), Authorize]
        public async Task<ActionResult> ChangePassword(ChangePasswordIn dto)
        {
            var userId = User.GetId();
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

            var result = await _userManager.ChangePasswordAsync(
                user,
                dto.Current,
                dto.NewPassword
            );

            if (result.Succeeded)
                return Ok("Password changed.");

            return BadRequest("Password not changed.");
        }

        /// <summary>
        /// Refresh a access token.
        /// </summary>
        [HttpPost("refresh-token"), AllowAnonymous]
        public async Task<ActionResult> RefreshToken(RefreshTokenIn dto)
        {
            var response = await VerifyToken(dto);

            if (response.Errors != null)
                return BadRequest(response);

            return Ok(response);
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

        #region Token things
        // TODO: refactor moving this to a service or manager.
        // TODO: add tests to methods.

        private void SetTokenValidationParameters()
        {
            _tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(_configuration["Jwt:SecurityKey"])
                ),

                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],

                ValidateLifetime = false,

                RoleClaimType = "role"
            };
        }

        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            return dateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
        }

        private async Task<RefreshTokenOut> VerifyToken(RefreshTokenIn dto)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var user = jwtTokenHandler.ValidateToken(dto.AccessToken, _tokenValidationParameters, out var validatedToken);

                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256);
                    if (result == false) return new RefreshTokenOut { Errors = new List<string>() { "Invalid access token algorithm." } };
                }

                var utcExpiryDate = long.Parse(user.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                var expDate = UnixTimeStampToDateTime(utcExpiryDate);
                if (expDate > DateTime.UtcNow)
                    return new RefreshTokenOut { Errors = new List<string>() {"Access token has not expired."} };

                var storedRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == dto.RefreshToken); 
                if (storedRefreshToken == null)
                    return new RefreshTokenOut { Errors = new List<string>() { "Refresh token doesnt exist." } };

                if (storedRefreshToken.IsExpired)
                    return new RefreshTokenOut { Errors = new List<string>() { "Refresh token has expired." } };

                if (storedRefreshToken.HasAlreadyBeenUsed)
                    return new RefreshTokenOut { Errors = new List<string>() { "Refresh token has already been used." } };

                if (storedRefreshToken.IsRevoked)
                    return new RefreshTokenOut { Errors = new List<string>() { "Refresh token has been revoked." } };

                var jti = user.Claims.First(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                if (storedRefreshToken.JwtId != jti)
                    return new RefreshTokenOut { Errors = new List<string>() {"Access and refresh tokens do not matches."} };

                storedRefreshToken.UsedAt = DateTime.UtcNow;

                _context.RefreshTokens.Update(storedRefreshToken);
                await _context.SaveChangesAsync();

                var userEmail = user.Claims.First(x => x.Type == JwtRegisteredClaimNames.Email).Value;

                var tokens = await GenerateLoginResponse(userEmail);

                var response = new RefreshTokenOut
                {
                    AccessToken = tokens.AccessToken,
                    RefreshToken = tokens.RefreshToken,
                    Errors = null
                };

                return response;
            }
            catch
            {
                return new RefreshTokenOut { Errors = new List<string>() { "Invalid access token." } };
            }
        }

        private string GetRefreshToken()
        {
            var randomNumber = new byte[42];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private async Task<LoginOut> GenerateLoginResponse(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            var claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
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
            var expirationTime = double.Parse(_configuration["Jwt:ExpirationTimeInMinutes"]);
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                Expires = DateTime.UtcNow.AddMinutes(expirationTime),
                SigningCredentials = signingCredentials,
                Subject = identityClaims
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(token);

            // Generate the refresh token

            var refreshTokenexpirationTime = double.Parse(_configuration["Jwt:RefreshTokenExpirationTimeInMinutes"]);

            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = GetRefreshToken(),
                JwtId = token.Id,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(refreshTokenexpirationTime),
                UsedAt = null,
                RevokedAt = null
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return new LoginOut
            {
                AccessToken = accessToken,
                TokenType = "Bearer",
                ExpiresInMinutes = expirationTime.ToString(),
                RefreshToken = refreshToken.Token,
                Scope = "create"
            };
        }

        #endregion
    }
}
