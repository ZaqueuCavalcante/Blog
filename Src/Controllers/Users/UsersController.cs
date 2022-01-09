using System.IdentityModel.Tokens.Jwt;
using Blog.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Blog.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Blog.Controllers.Users
{
    [ApiController, Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly TokenManager _tokenManager;
        private IConfiguration _configuration;

        public UsersController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            TokenManager tokenManager,
            IConfiguration configuration
        ) {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenManager = tokenManager;
            _configuration = configuration;
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
                var (accessToken, refreshToken) = await _tokenManager.GenerateTokens(dto.Email);

                var response =  new LoginOut
                {
                    AccessToken = accessToken,
                    TokenType = "Bearer",
                    ExpiresInMinutes = _configuration["Jwt:ExpirationTimeInMinutes"],
                    RefreshToken = refreshToken,
                    Scope = "create"
                };

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

            await _tokenManager.RevokeRefreshTokens(User.GetId());

            return Ok("Logout succeeded.");
        }

        /// <summary>
        /// Reset a forgotten password.
        /// </summary>
        [HttpPost("generate-reset-password-token"), AllowAnonymous]
        public async Task<ActionResult> GenerateResetPasswordToken([Required, EmailAddress] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound("Email not found.");
 
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // TODO: SEND TO USER EMAIL ***********

            return Ok(token);
        }

        /// <summary>
        /// Change user password.
        /// </summary>
        [HttpPatch("change-password"), Authorize]
        public async Task<ActionResult> ChangePassword(ChangePasswordIn dto)
        {
            var userId = User.GetId();

            var error = await _tokenManager.FindUnunsedRefreshTokens(userId);
            if (error != null)
                return BadRequest(error);

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
            var error = await _tokenManager.TryUseRefreshToken(dto.AccessToken, dto.RefreshToken);
            if (error != null)
                return BadRequest(error);

            var tokenHandler = new JwtSecurityTokenHandler();
            var userEmail = tokenHandler.ReadJwtToken(dto.AccessToken)
                .Claims.First(x => x.Type == JwtRegisteredClaimNames.Email).Value;

            var (accessToken, refreshToken) = await _tokenManager.GenerateTokens(userEmail);
            var response = new RefreshTokenOut
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return Ok(response);
        }
    }
}
