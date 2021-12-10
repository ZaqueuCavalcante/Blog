using Blog.Exceptions;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Blog.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Blog.Identity;
using System.Reflection;

namespace Blog
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = null);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Blog",
                    Version = "1.0",
                    Description = "A API to a simple blog engine."
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            });

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddDbContext<BlogContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("Connection"));
                options.UseSnakeCaseNamingConvention();
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            });

            services.AddCors();

            // services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
            // {
            //     options.Authority = "https://localhost:5001";

            //     options.TokenValidationParameters = new TokenValidationParameters
            //     {
            //         ValidateAudience = false
            //     };
            // });

            // services.AddAuthorization(options =>
            // {
            //     options.AddPolicy("ApiScope", policy =>
            //     {
            //         policy.RequireAuthenticatedUser();
            //         policy.RequireClaim("scope", "api1");
            //     });
            // });


            // Identity configurations
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<BlogContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);  // The amount of time a user is locked out when a lockout occurs.
                options.Lockout.MaxFailedAccessAttempts = 5;  // The number of failed access attempts until a user is locked out, if lockout is enabled.
                options.Lockout.AllowedForNewUsers = true;  // Determines if a new user can be locked out.
            });

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 6;  // The minimum length.
                options.Password.RequireDigit = true;  // Requires a number between 0-9.
                options.Password.RequireUppercase = true;  // Requires an uppercase character.
                options.Password.RequireLowercase = true;  // Requires a lowercase character.
                options.Password.RequireNonAlphanumeric = true;  // Requires a non-alphanumeric character (@, %, #, !, &, $, ...).
                options.Password.RequiredUniqueChars = 1;  // Requires the number of distinct characters.
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

            services.Configure<IdentityOptions>(options =>
            {
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            services.Configure<CookieAuthenticationOptions>(options =>
            {
                // options.AccessDeniedPath = "/identity/account/access-denied";  // Used by the handler for the redirection target when handling ForbidAsync.
                // options.ClaimsIssuer = "";  // The issuer that should be used for any claims that are created.
                // options.Cookie.Name = "YourAppCookieName";
                // options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                // options.LoginPath = "/identity/account/login";
                // options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                // options.SlidingExpiration = true;
            });

            services.Configure<PasswordHasherOptions>(options =>
            {
                // options.IterationCount = 10_000;
                // options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3;
            });
        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            BlogContext context
        ) {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog 1.0"));

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                DbSeeder.Seed(context);
            }

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()); 

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<DomainExceptionMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                    // .RequireAuthorization("ApiScope");  // Para todos os endpoints...
            });
        }
    }
}
