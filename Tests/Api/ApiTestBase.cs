using System.Net.Http.Headers;
using System.Security.Claims;
using Blog.Controllers.Users;
using Blog.Database;
using Blog.Domain;
using Blog.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using static Blog.Configurations.AuthorizationConfigurations;

namespace Blog.Tests.Api;

public class ApiTestBase
{
    protected HttpClient _client;
    protected APIWebApplicationFactory _factory;

    private BlogContext _context;
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
        _client = _factory.CreateClient();

        using (var scope = _factory.Services.CreateScope())
        {
            _context = scope.ServiceProvider.GetRequiredService<BlogContext>();
            _userManager = scope.ServiceProvider.GetRequiredService<UserManager<BlogUser>>();
            _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            await Seed();
        }
    }

    protected async Task Login(string email, string password)
    {
        var userIn = new UserIn { Email = email, Password = password };
        var loginResponse = await _client.PostAsync("users/login", userIn.ToStringContent());
        var loginOut = JsonConvert.DeserializeObject<LoginOut>(await loginResponse.Content.ReadAsStringAsync());
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginOut.AccessToken);    
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
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        #region Bloggers
        
        var samBlogger = new Blogger("Sam Esmail", "Writes about ASP.NET Core, DevOps and TV Shows.", samUser.Id);
        await _context.Bloggers.AddAsync(samBlogger);
        await _context.SaveChangesAsync();

        var elliotBlogger = new Blogger("Elliot Alderson", "Writes about Linux, Hacking and Computers.", elliotUser.Id);
        await _context.Bloggers.AddAsync(elliotBlogger);
        await _context.SaveChangesAsync();

        var irvingBlogger = new Blogger("Irving", "A used-car salesman, that writes novels.", irvingUser.Id);
        await _context.Bloggers.AddAsync(irvingBlogger);
        await _context.SaveChangesAsync();

        #endregion
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        #region Networks

        samUser.Networks = new List<Network>
        {
            new Network("YouTube", "https://www.youtube.com/sam"),
            new Network("Twitter", "https://twitter.com/sam")
        };

        irvingUser.Networks = new List<Network>
        {
            new Network("YouTube", "https://www.youtube.com/irving"),
            new Network("Twitter", "https://twitter.com/irving")
        };

        await _context.SaveChangesAsync();

        #endregion
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        #region Tags

        var techTag = new Tag("Tech");
        await _context.Tags.AddAsync(techTag);
        await _context.SaveChangesAsync();

        var seriesTag = new Tag("Series");
        await _context.Tags.AddAsync(seriesTag);
        await _context.SaveChangesAsync();

        var hackingTag = new Tag("Hacking");
        await _context.Tags.AddAsync(hackingTag);
        await _context.SaveChangesAsync();

        #endregion
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        #region Categories

        var linuxCategory = new Category("Linux", "The Linux category description.");
        await _context.Categories.AddAsync(linuxCategory);
        await _context.SaveChangesAsync();

        var mrRobotCategory = new Category("Mr. Robot", "The Mr. Robot category description.");
        await _context.Categories.AddAsync(mrRobotCategory);
        await _context.SaveChangesAsync();

        var efCoreCategory = new Category("EF Core", "The EF Core category description.");
        await _context.Categories.AddAsync(efCoreCategory);
        await _context.SaveChangesAsync();

        var identityCategory = new Category("Identity Core", "The Identity Core category description.");
        await _context.Categories.AddAsync(identityCategory);
        await _context.SaveChangesAsync();

        var swaggerCategory = new Category("Swagger", "The Swagger category description.");
        await _context.Categories.AddAsync(swaggerCategory);
        await _context.SaveChangesAsync();

        #endregion
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        #region Posts

        var mrRobotpost = new Post(
            title: "Mr. Robot - End explained",
            resume: "The Mr. Robot series finale features a stunning, emotional twist that recontextualizes the entire series.",
            body: "Obviously he is a fictional character given that he exists on the television program Mr. Robot. But even within the reality of the show, Elliot at times more closely resembled a hacker archetype than a living, breathing, bleeding human being.",
            categoryId: mrRobotCategory.Id,
            authorId: samBlogger.Id,
            tags: new List<Tag>{ seriesTag, hackingTag }
        );
        await _context.Posts.AddAsync(mrRobotpost);
        await _context.SaveChangesAsync();

        var linuxPost = new Post(
            title: "Linux and hacking",
            resume: "Linux Basics for Hackers: Getting Started with Networking, Scripting, and Security in Kali.",
            body: "This practical, tutorial-style book uses the Kali Linux distribution to teach Linux basics with a focus on how hackers would use them. Topics include Linux command line basics, filesystems, networking, BASH basics, package management, logging, and the Linux kernel and drivers.",
            categoryId: linuxCategory.Id,
            authorId: elliotBlogger.Id,
            tags: new List<Tag>{ techTag, hackingTag }  
        );
        await _context.Posts.AddAsync(linuxPost);
        await _context.SaveChangesAsync();

        var efCorePost = new Post(
            title: "EF Core - Code First",
            resume: "Code first approach offers most control over the final appearance of the application code and the resulting database.",
            body: "In EF Core, the DbContext has a virtual method called onConfiguring which will get called internally by EF Core, and it will also pass in an optionsBuilder instance, and you can use that optionsBuilder to configure options for the DbContext.",
            categoryId: efCoreCategory.Id,
            authorId: irvingBlogger.Id,
            tags: new List<Tag>{ techTag }
        );
        await _context.Posts.AddAsync(efCorePost);
        await _context.SaveChangesAsync();

        #endregion
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        #region Readers

        var darleneReader = new Reader("Darlene Alderson", darleneUser.Id);
        await _context.Readers.AddAsync(darleneReader);
        await _context.SaveChangesAsync();

        var tyrellReader = new Reader("Tyrell Wellick", tyrellUser.Id);
        await _context.Readers.AddAsync(tyrellReader);
        await _context.SaveChangesAsync();

        var angelaReader = new Reader("Angela Moss", angelaUser.Id);
        await _context.Readers.AddAsync(angelaReader);
        await _context.SaveChangesAsync();

        var domReader = new Reader("Dominique DiPierro", domUser.Id);
        await _context.Readers.AddAsync(domReader);
        await _context.SaveChangesAsync();

        #endregion
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        #region Comments

        var mrRobotPostComment01 = new Comment(mrRobotpost.Id, 5, "Great first season.", darleneUser.Id);
        var mrRobotPostComment02 = new Comment(mrRobotpost.Id, 4, "Amazing.", tyrellUser.Id);
        var mrRobotPostComment03 = new Comment(mrRobotpost.Id, 3, "More tath a hacker show.", angelaUser.Id);
        await _context.Comments.AddRangeAsync(mrRobotPostComment01, mrRobotPostComment02, mrRobotPostComment03);
        await _context.SaveChangesAsync();

        var linuxPostComment01 = new Comment(linuxPost.Id, 5, "Very useful.", tyrellUser.Id);
        var linuxPostComment02 = new Comment(linuxPost.Id, 5, "Interesting...", samUser.Id);
        await _context.Comments.AddRangeAsync(linuxPostComment01, linuxPostComment02);
        await _context.SaveChangesAsync();

        #endregion
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        #region Replies

        var reply01 = new Reply(mrRobotPostComment01.Id, "A comment reply.", samUser.Id);
        var reply02 = new Reply(mrRobotPostComment01.Id, "A other comment reply.", elliotUser.Id);
        var reply03 = new Reply(mrRobotPostComment02.Id, "A reply lalala.", angelaUser.Id);
        await _context.Replies.AddRangeAsync(reply01, reply02, reply03);
        await _context.SaveChangesAsync();

        #endregion
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        #region Likes

        var like01 = new Like(mrRobotPostComment01.Id, darleneUser.Id);
        var like02 = new Like(mrRobotPostComment01.Id, tyrellUser.Id);
        var like03 = new Like(mrRobotPostComment01.Id, angelaUser.Id);
        var like04 = new Like(mrRobotPostComment02.Id, tyrellUser.Id);
        var like05 = new Like(linuxPostComment01.Id, samUser.Id);
        var like06 = new Like(linuxPostComment02.Id, elliotUser.Id);
        await _context.Likes.AddRangeAsync(like01, like02, like03, like04, like05, like06);
        await _context.SaveChangesAsync();

        #endregion
    }
}
