using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Blog.Configurations
{
    public static class JwtConfigurations
    {
        public static IServiceCollection AddJwtConfigurations(
            this IServiceCollection services,
            IConfiguration configuration
        ) {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = configuration["Jwt:Issuer"],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(configuration["Jwt:SecurityKey"])
                ),

                ValidateAudience = true,
                ValidAudience = configuration["Jwt:Audience"],

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,

                RoleClaimType = "role"
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer("Bearer", options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = tokenValidationParameters;
            });

            return services;
        }
    }
}
