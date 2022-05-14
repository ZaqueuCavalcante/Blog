using Blog.Auth;
using Blog.Database;
using Blog.Domain;
using Blog.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Blog.Configurations.AuthorizationConfigurations;

namespace Blog.Services.Bloggers;

public class BloggersService : IBloggersService
{
    private readonly BlogContext _context;
    private readonly UserManager<BlogUser> _userManager;

    public BloggersService(
        BlogContext context,
        UserManager<BlogUser> userManager
    ) {
        _context = context;
        _userManager = userManager;
    }

    public async Task<Blogger> CreateBlogger(string name, string resume, string email, string password)
    {
        var user = new BlogUser(email);

        var blogger = new Blogger(name, resume, user.Id);

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            throw new DomainException("Erro ao criar blogger.");

        await _userManager.AddToRoleAsync(user, BloggerRole);

        blogger.UserId = user.Id;

        _context.Bloggers.Add(blogger);
        await _context.SaveChangesAsync();

        return blogger;
    }

    public async Task<Blogger> GetBlogger(int id)
    {
        var blogger = await _context.Bloggers.AsNoTracking()
            .Include(b => b.Posts)
            .Include(b => b.Networks)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (blogger is null)
            throw new DomainException("Blogger not found.", 404);
        
        return blogger;
    }

    public async Task<List<Blogger>> GetBloggers()
    {
        var bloggers = await _context.Bloggers.AsNoTracking()
            .ToListAsync();

        return bloggers;
    }

    public async Task UpdateBlogger(int userId, string name, string resume)
    {
        var blogger = await _context.Bloggers.FirstAsync(b => b.UserId == userId);

        blogger.Update(name, resume);

        await _context.SaveChangesAsync();
    }
}
