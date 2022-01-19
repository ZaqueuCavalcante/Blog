using System.Linq.Expressions;
using Blog.Database;
using Blog.Domain;
using Blog.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Blog.Configurations.AuthorizationConfigurations;

namespace Blog.Controllers.Posts
{
    [ApiController, Route("[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly BlogContext _context;

        public PostsController(BlogContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Create a new blog post.
        /// </summary>
        [HttpPost, Authorize(Roles = BloggerRole)]
        public async Task<IActionResult> PostPost(PostIn dto)
        {
            var author = await _context.Bloggers.FirstOrDefaultAsync(b => b.UserId == User.GetId());

            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == dto.CategoryId);
            if (category == null)
                return NotFound("Category not found.");

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

            return Created($"/posts/{post.Id}", PostOut.New(post));
        }

        /// <summary>
        /// Update a blog post.
        /// </summary>
        [HttpPatch("{id}"), Authorize(Roles = BloggerRole)]
        public async Task<IActionResult> EditPost(int id, EditPostIn dto)
        {
            var post = await _context.Posts.Include(p => p.Author).FirstOrDefaultAsync(p => p.Id == id);
            if (post is null)
                return NotFound("Post not found.");

            if (post.Author.UserId != User.GetId())
                return Forbid("You must be the post author to be able to edit it.");

            post.Edit(dto.Title, dto.Resume, dto.Body);

            await _context.SaveChangesAsync();

            return Ok(PostOut.NewWithoutComments(post));
        }

        /// <summary>
        /// Comment on a blog post.
        /// </summary>
        [HttpPost("{postId}/comments"), Authorize(Roles = $"{ReaderRole}, {BloggerRole}")]
        public async Task<IActionResult> PostComment(int postId, CommentIn dto)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            if (post is null)
                return NotFound("Post not found.");

            var comment = new Comment(postId, dto.PostRating, dto.Body, User.GetId());

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return Created($"/posts/{post.Id}/comments/{comment.Id}", CommentOut.New(comment));
        }

        /// <summary>
        /// Get a comment from a blog post.
        /// </summary>
        [HttpGet("{postId}/comments/{commentId}"), AllowAnonymous]
        public async Task<IActionResult> GetComment(int postId, int commentId)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(
                c => c.PostId == postId && c.Id == commentId
            );

            if (comment is null)
                return NotFound("Comment not found.");

            return Ok(CommentOut.New(comment));
        }

        /// <summary>
        /// Pins a comment to a blog post. If the comment already is pinned, it will be unpinned.
        /// </summary>
        [HttpPatch("{postId}/comments/{commentId}/pins"), Authorize(Policy = CommentPinPolicy)]
        public async Task<IActionResult> PostCommentPin(int postId, int commentId)
        {
            var post = await _context.Posts.Include(p => p.Author).FirstOrDefaultAsync(p => p.Id == postId);
            if (post is null)
                return NotFound("Post not found.");

            if (post.Author.UserId != User.GetId())
                return Forbid("You must be the post author to be able to pin a comment.");

            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId && c.PostId == post.Id);
            if (comment is null)
                return NotFound("Comment not found.");

            post.Pin(comment.Id);

            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Reply to a comment on a blog post.
        /// </summary>
        [HttpPost("{postId}/comments/{commentId}/replies"), Authorize(Roles = $"{ReaderRole}, {BloggerRole}")]
        public async Task<IActionResult> PostCommentReply(int postId, int commentId, ReplyIn dto)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            if (post is null)
                return NotFound("Post not found.");

            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId && c.PostId == post.Id);
            if (comment is null)
                return NotFound("Comment not found.");

            var reply = new Reply(comment.Id, dto.Body, User.GetId());

            _context.Replies.Add(reply);
            await _context.SaveChangesAsync();

            return Created($"/posts/{post.Id}/comments/{comment.Id}/replies/{reply.Id}", ReplyOut.New(reply));
        }

        /// <summary>
        /// Like/dislike a comment on a blog post.
        /// </summary>
        [HttpPost("{postId}/comments/{commentId}/likes"), Authorize(Policy = CommentLikePolicy)]
        public async Task<IActionResult> PostCommentLike(int postId, int commentId)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            if (post is null)
                return NotFound("Post not found.");

            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId && c.PostId == post.Id);
            if (comment is null)
                return NotFound("Comment not found.");

            var userId = User.GetId();

            var like = await _context.Likes.FirstOrDefaultAsync(l => l.CommentId == commentId && l.UserId == userId);

            if (like != null)
            {
                _context.Likes.Remove(like);
                await _context.SaveChangesAsync();
                return Ok("Like removed.");
            }

            like = new Like(comment.Id, userId);

            _context.Likes.Add(like);
            await _context.SaveChangesAsync();

            return Created($"/posts/{post.Id}/comments/{comment.Id}/likes/{like.Id}", LikeOut.New(comment.Id, userId));
        }

        /// <summary>
        /// Returns a post.
        /// </summary>
        [HttpGet("{id}"), AllowAnonymous]
        public async Task<ActionResult<PostOut>> GetPost(int id)
        {
            var post = await _context.Posts
                .AsNoTrackingWithIdentityResolution()
                .Include(p => p.Category)
                .Include(l => l.Author)
                .Include(l => l.Comments)
                    .ThenInclude(c => c.Replies.OrderBy(r => r.CreatedAt))
                .Include(l => l.Comments)
                    .ThenInclude(c => c.Likes)
                .Include(l => l.Tags)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (post is null)
                return NotFound("Post not found.");

            return Ok(PostOut.New(post, Request.GetRoot()));
        }

        /// <summary>
        /// Returns some posts.
        /// </summary>
        [HttpGet, AllowAnonymous]
        public async Task<ActionResult<List<PostOut>>> GetPosts([FromQuery] PostParameters parameters)
        {
            if (parameters.MinDateIsGreaterThanMaxDate())
                return BadRequest("MinDate must be less than MaxDate.");

            Expression<Func<Post, bool>> predicate = p =>
                (p.Tags.Any(t => t.Id == parameters.TagId || parameters.TagId == null)) &&
                (p.Category.Id == parameters.CategoryId || parameters.CategoryId == null) &&
                (p.CreatedAt >= parameters.MinDate || parameters.MinDate == null) &&
                (p.CreatedAt <= parameters.MaxDate || parameters.MaxDate == null);

            var posts = await _context.Posts
                .AsNoTrackingWithIdentityResolution()
                .Include(p => p.Category)
                .Include(l => l.Author)
                .Include(l => l.Comments)
                .Include(l => l.Tags)
                .Where(predicate)
                .OrderByDescending(p => p.CreatedAt)
                .Page(parameters)
                .ToListAsync();

            var count = await _context.Posts.Where(predicate).CountAsync();

            Response.AddPagination(parameters, count);

            return Ok(posts.Select(p => PostOut.NewWithoutComments(p, Request.GetRoot())).ToList());
        }
    }
}
