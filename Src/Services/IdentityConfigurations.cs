using System.Text;
using Blog.Database;
using Blog.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Bolg.Services
{
    public static class IdentityConfigurations
    {
        public static IServiceCollection AddIdentityConfigurations(
            this IServiceCollection services,
            IConfiguration configuration
        ) {
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<BlogContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 6;  // The minimum length.
                options.Password.RequireDigit = true;  // Requires a number between 0-9.
                options.Password.RequireLowercase = true;  // Requires a lowercase character.
                options.Password.RequireUppercase = true;  // Requires an uppercase character.
                options.Password.RequireNonAlphanumeric = true;  // Requires a non-alphanumeric character (@, %, #, !, &, $, ...).
                options.Password.RequiredUniqueChars = 1;  // Requires the minimum number of distinct characters.
            });

            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);  // The amount of time a user is locked out when a lockout occurs.
                options.Lockout.MaxFailedAccessAttempts = 5;  // The number of failed access attempts until a user is locked out, if lockout is enabled.
                options.Lockout.AllowedForNewUsers = true;  // Determines if a new user can be locked out.
            });

            services.Configure<IdentityOptions>(options =>
            {
                // options.SignIn.RequireConfirmedEmail = false;  // Requires a confirmed email to sign in.
                // options.SignIn.RequireConfirmedAccount = false;  // Requires a confirmed account to sign in.
                // options.SignIn.RequireConfirmedPhoneNumber = false;  // Requires a confirmed phone number to sign in.
            });

            services.Configure<IdentityOptions>(options =>
            {
                // options.Tokens.AuthenticatorIssuer = "";  // Pra q serve isso?
                // options.Tokens.AuthenticatorTokenProvider = "";  // Used to validate two-factor sign-ins with an authenticator.
                // options.Tokens.ChangeEmailTokenProvider = "";  // Used to generate tokens used in email change confirmation emails.
                // options.Tokens.ChangePhoneNumberTokenProvider = "";  // Used to generate tokens used when changing phone numbers.
                // options.Tokens.EmailConfirmationTokenProvider = "";  // Used to generate tokens used in account confirmation emails.
                // options.Tokens.PasswordResetTokenProvider = "";  // Used to generate tokens used in password reset emails.
                // options.Tokens.ProviderMap = "";  // Used to construct a User Token Provider with the key used as the provider's name.
            });

            services.Configure<CookieAuthenticationOptions>(options =>
            {
                options.LoginPath = "/identity/users/login";
                options.LogoutPath = "/identity/users/logout";
                options.AccessDeniedPath = "/identity/users/forbidden";  // Used by the handler for the redirection target when handling ForbidAsync.

                // options.ClaimsIssuer = "";  // The issuer that should be used for any claims that are created.
                // options.Cookie.Name = "YourAppCookieName";
                // options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                // options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                // options.SlidingExpiration = true;
            });

            services.Configure<PasswordHasherOptions>(options =>
            {
                // options.IterationCount = 10_000;
                // options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3;
            });




            services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
            {
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Jwt:Issuer"],

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(configuration["Jwt:SecurityKey"])
                    ),

                    ValidateAudience = true,
                    ValidAudience = configuration["Jwt:Audience"],

                    ValidateLifetime = true
                };
            });

            return services;
        }
    }
}
