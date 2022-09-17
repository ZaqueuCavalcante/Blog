using Blog.Controllers.Posts;
using Blog.Database;
using Blog.Domain;
using Blog.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Blog.Services.Posts;

public class PostsService : IPostsService
{
    private readonly BlogContext _context;

    public PostsService(BlogContext context) => _context = context;

    public async Task<Post> CreatePost(int userId, PostIn dto)
    {
        var author = await _context.Bloggers.FirstOrDefaultAsync(b => b.UserId == userId);

        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == dto.CategoryId);
        if (category == null)
            throw new DomainException("Category not found.", 404);

        List<Tag>? tags = null;
        if (dto.Tags != null && dto.Tags.Any())
            tags = await _context.Tags.Where(t => dto.Tags.Contains(t.Id)).ToListAsync();

        var post = new Post(
            dto.Title,
            dto.Resume,
            dto.Body,
            category.Id,
            author.Id,
            tags
        );

        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        return post;
    }
}
