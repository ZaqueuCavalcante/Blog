using System.Security.Claims;
using Blog.Database;
using Blog.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using static Blog.Configurations.AuthorizationConfigurations;

namespace Blog.Tests.Integration;

public class IntegrationTestBase
{
    protected APIWebApplicationFactory _factory;

    protected BlogContext _context;
    protected TokenManager _tokenManager;
    private UserManager<User> _userManager;
    private RoleManager<Role> _roleManager;

    [OneTimeSetUp]
    public virtual void OneTimeSetUp()
    {
        _factory = new APIWebApplicationFactory();
    }

    [SetUp]
    public async Task SetupBeforeEachTest()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            _context = scope.ServiceProvider.GetRequiredService<BlogContext>();
            _tokenManager = scope.ServiceProvider.GetRequiredService<TokenManager>();
            _userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            await Seed();
        }
    }

    private async Task Seed()
    {
        #region Roles

        var readerRole = new Role { Name = ReaderRole };
        var bloggerRole = new Role { Name = BloggerRole };
        var adminRole = new Role { Name = AdminRole };

        await _roleManager.CreateAsync(readerRole);
        await _roleManager.CreateAsync(bloggerRole);
        await _roleManager.CreateAsync(adminRole);

        #endregion
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        #region Users

        var samUser = Blog.Identity.User.New("sam@blog.com");
        var elliotUser = Blog.Identity.User.New("elliot@blog.com");
        var irvingUser = Blog.Identity.User.New("irving@blog.com");
        var darleneUser = Blog.Identity.User.New("darlene@blog.com");
        var tyrellUser = Blog.Identity.User.New("tyrell@blog.com");
        var angelaUser = Blog.Identity.User.New("angela@blog.com");
        var domUser = Blog.Identity.User.New("dom@blog.com");

        await _userManager.CreateAsync(samUser, "Test@123");
        await _userManager.CreateAsync(elliotUser, "Test@123");
        await _userManager.CreateAsync(irvingUser, "Test@123");
        await _userManager.CreateAsync(darleneUser, "Test@123");
        await _userManager.CreateAsync(tyrellUser, "Test@123");
        await _userManager.CreateAsync(angelaUser, "Test@123");
        await _userManager.CreateAsync(domUser, "Test@123");

        await _userManager.AddToRolesAsync(samUser, new [] { AdminRole, BloggerRole });
        await _userManager.AddToRoleAsync(elliotUser, BloggerRole);
        await _userManager.AddToRoleAsync(irvingUser, BloggerRole);
        await _userManager.AddToRoleAsync(darleneUser, ReaderRole);
        await _userManager.AddToRoleAsync(tyrellUser, ReaderRole);
        await _userManager.AddToRoleAsync(angelaUser, ReaderRole);
        await _userManager.AddToRoleAsync(domUser, ReaderRole);

        #endregion
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        #region Claims

        await _userManager.AddClaimAsync(elliotUser, new Claim("pinner", "true"));
        await _userManager.AddClaimAsync(irvingUser, new Claim("liker", "true"));
        await _userManager.AddClaimAsync(darleneUser, new Claim("liker", "true"));

        #endregion

        await _context.SaveChangesAsync();
    }
}
