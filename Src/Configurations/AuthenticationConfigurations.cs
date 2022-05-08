using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Blog.Auth;
using Blog.Settings;
using Microsoft.IdentityModel.Tokens;
using static Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults;

namespace Blog.Configurations;

public static class AuthenticationConfigurations
{
    public static void AddAuthenticationConfigurations(
        this IServiceCollection services
    ) {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        var jwtSettings = services.BuildServiceProvider().GetService<JwtSettings>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = AuthenticationScheme;
            options.DefaultScheme = AuthenticationScheme;
            options.DefaultChallengeScheme = AuthenticationScheme;
        })
        .AddJwtBearer(AuthenticationScheme, options =>
        {
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(jwtSettings.SecurityKey)
                ),

                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience,

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,

                RoleClaimType = "role"
            };
        });

        services.AddScoped<TokenManager>();
    }
}
