using System.Security.Claims;
using Blog.Database;
using Blog.Auth;
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
    private UserManager<BlogUser> _userManager;
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
            _userManager = scope.ServiceProvider.GetRequiredService<UserManager<BlogUser>>();
            _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            await Seed();
        }
    }

    private async Task Seed()
    {
        #region Users

        var samUser = new BlogUser("sam@blog.com");
        var elliotUser = new BlogUser("elliot@blog.com");
        var irvingUser = new BlogUser("irving@blog.com");
        var darleneUser = new BlogUser("darlene@blog.com");
        var tyrellUser = new BlogUser("tyrell@blog.com");
        var angelaUser = new BlogUser("angela@blog.com");
        var domUser = new BlogUser("dom@blog.com");

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

        await _userManager.AddClaimAsync(elliotUser, new Claim(PinnerClaim, "true"));
        await _userManager.AddClaimAsync(irvingUser, new Claim(LikerClaim, "true"));
        await _userManager.AddClaimAsync(darleneUser, new Claim(LikerClaim, "true"));

        #endregion

        await _context.SaveChangesAsync();
    }
}
