using System.IdentityModel.Tokens.Jwt;
using Blog.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Blog.Extensions;
using Blog.Settings;

namespace Blog.Controllers.Users;

[ApiController, Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserManager<BlogUser> _userManager;
    private readonly SignInManager<BlogUser> _signInManager;
    private readonly TokenManager _tokenManager;

    public UsersController(
        UserManager<BlogUser> userManager,
        SignInManager<BlogUser> signInManager,
        TokenManager tokenManager
    ) {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenManager = tokenManager;
    }

    /// <summary>
    /// Login into blog.
    /// </summary>
    [HttpPost("login"), AllowAnonymous]
    public async Task<ActionResult> Login(UserIn dto, [FromServices] JwtSettings jwtSettings)
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
                ExpiresInMinutes = jwtSettings.ExpirationTimeInMinutes.ToString(),
                RefreshToken = refreshToken,
                Scope = "create"
            };

            return Ok(response);
        }

        if (result.IsLockedOut)
            return Unauthorized("Account locked.");

        if (result.IsNotAllowed)
            return Unauthorized("Login not allowed.");

        if (result.RequiresTwoFactor)
            return Unauthorized("Requires two factor.");
        
        return Unauthorized("Login failed.");
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
    [HttpPost("reset-password"), AllowAnonymous]  // TODO: Add email token sent
    public async Task<ActionResult> ResetPassword(ResetPasswordIn dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            return NotFound("Email not found.");

        var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);
        if (!result.Succeeded)
            return BadRequest("Reset password failed.");

        return Ok("Password reseted!");
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
