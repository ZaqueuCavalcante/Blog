using Blog.Database;
using Blog.Domain;
using Blog.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers.Posts
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly BlogContext _context;

        public PostsController(BlogContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> PostPost(PostIn dto)
        {
            var authors = await _context.Bloggers.Where(x => dto.Authors.Contains(x.Id)).ToListAsync();
            if (authors.Count == 0)
                throw new DomainException("Author not found.");

            var tags = await _context.Tags.Where(x => dto.Tags.Contains(x.Name)).ToListAsync();
            var newTags = dto.Tags.Except(tags.Select(x => x.Name))
                .Select(x => new Tag { Name = x, CreatedAt = DateTime.Now }).ToList();
            tags.AddRange(newTags);

            var post = new Post
            {
                Title = dto.Title,
                Resume = dto.Resume,
                Body = dto.Body,
                CreatedAt = DateTime.Now,
                Authors = authors,
                Tags = tags
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return Created($"/posts/{post.Id}", new PostOut(post));
        }

        [HttpPost("{id}/comments")]
        public async Task<IActionResult> PostComment(int id, CommentIn dto)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
            if (post is null)
                throw new DomainException("Post not found.");

            var reader = await _context.Readers.FirstOrDefaultAsync(r => r.Id == dto.ReaderId);
            var blogger = await _context.Bloggers.FirstOrDefaultAsync(b => b.Id == dto.BloggerId);
            if (reader is null && blogger is null)
                throw new DomainException("Commenter not found.");

            if (reader is not null && blogger is not null)
                throw new DomainException("A comment must have a single commenter.");

            var comment = new Comment
            {
                Body = dto.Body,
                CreatedAt = DateTime.Now,
                PostId = id,
                ReaderId = dto.ReaderId,
                BloggerId = dto.BloggerId
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return Created($"/posts/{post.Id}", new PostOut(post));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostOut>> GetPost(int id)
        {
            var post = await _context.Posts
                .AsNoTrackingWithIdentityResolution()
                .Include(l => l.Authors)
                .Include(l => l.Comments).ThenInclude(c => c.Replies)
                .Include(l => l.Tags)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (post is null)
                return NotFound("Post not found.");

            return Ok(new PostOut(post));
        }

        [HttpGet]
        public async Task<ActionResult<List<PostOut>>> GetPosts([FromQuery] string? tag)
        {
            var posts = await _context.Posts
                .AsNoTrackingWithIdentityResolution()
                .Include(l => l.Authors)
                .Include(l => l.Comments).ThenInclude(c => c.Replies)
                .Include(l => l.Tags)
                .Where(p => p.Tags.Any(t => t.Name == tag) || string.IsNullOrEmpty(tag))
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return Ok(posts.Select(x => new PostOut(x)).ToList());
        }
    }
}
