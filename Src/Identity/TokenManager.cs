using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Blog.Database;

namespace Blog.Identity
{
    public class TokenManager
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly BlogContext _context;
        private IConfiguration _configuration;
        private TokenValidationParameters _tokenValidationParameters;

        public TokenManager(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            BlogContext context,
            IConfiguration configuration
        ) {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _configuration = configuration;
            SetTokenValidationParameters();
        }

        public async Task<(string accessToken, string refreshToken)> GenerateTokens(string userEmail)
        {
            var accessToken = await GenerateAccessToken(userEmail);
            var refreshToken = await GenerateRefreshToken(userEmail, accessToken);
            return (accessToken, refreshToken);
        }

        public async Task<string?> TryUseRefreshToken(string accessToken, string refreshToken)
        {
            try
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var user = jwtTokenHandler.ValidateToken(accessToken, _tokenValidationParameters, out var validatedToken);

                if (GetExpDate(user.Claims) > DateTime.UtcNow)
                    return "Access token has not expired.";

                var storedRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken); 
                if (storedRefreshToken == null)
                    return "Refresh token doesnt exist.";

                var jti = user.Claims.First(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                var error = storedRefreshToken.TryUse(jti);
                if (error != null)
                    return error;

                await _context.SaveChangesAsync();

                return null;
            }
            catch
            {
                return "Invalid access token.";
            }
        }

        public async Task RevokeRefreshTokens(int userId)
        {
            var refreshTokens = await _context.RefreshTokens.Where(t =>
                t.UserId == userId && t.UsedAt == null && t.RevokedAt == null).ToListAsync();

            refreshTokens.ForEach(t => t.Revoke());

            await _context.SaveChangesAsync();
        }

        public async Task<string?> FindUnunsedRefreshTokens(int userId)
        {
            var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(t =>
                t.UserId == userId && t.UsedAt == null && t.RevokedAt == null);

            if (refreshToken == null)
                return "Make login again.";

            return null;
        }

        private async Task<string> GenerateAccessToken(string email)
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

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private async Task<string> GenerateRefreshToken(string userEmail, string accessToken)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtId = tokenHandler.ReadJwtToken(accessToken)
                .Claims.First(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

            var refreshTokenexpirationTime = double.Parse(_configuration["Jwt:RefreshTokenExpirationTimeInMinutes"]);

            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = GenerateRandomBase64(),
                JwtId = jwtId,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(refreshTokenexpirationTime),
                UsedAt = null,
                RevokedAt = null
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return refreshToken.Token;
        }

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

        private DateTime GetExpDate(IEnumerable<Claim> userClaims)
        {
            var utcExpiryDate = long.Parse(userClaims.First(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            return dateTime.AddSeconds(utcExpiryDate).ToUniversalTime();
        }

        private string GenerateRandomBase64(int length = 42)
        {
            var randomNumber = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
