using Blog.Database;
using Blog.Domain;
using Blog.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Blog.Configurations.AuthorizationConfigurations;

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

        /// <summary>
        /// Create a new blog post.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Blogger")]
        public async Task<IActionResult> PostPost(PostIn dto)
        {
            var userId = User.GetId();
            var mainAuthor = await _context.Bloggers.FirstOrDefaultAsync(b => b.UserId == userId);

            var authors = new List<Blogger>();
            if (dto.Authors != null && dto.Authors.Any())
                authors = await _context.Bloggers.Where(x => dto.Authors.Contains(x.Id)).ToListAsync();

            authors.Add(mainAuthor);

            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == dto.Category);
            if (category == null)
                return NotFound("Category not found.");

            List<Tag>? tags = null;
            if (dto.Tags != null && dto.Tags.Any())
                tags = await _context.Tags.Where(x => dto.Tags.Contains(x.Name)).ToListAsync();

            var post = new Post
            {
                Title = dto.Title,
                Resume = dto.Resume,
                Body = dto.Body,
                CategoryId = category.Id,
                CreatedAt = DateTime.Now,
                Authors = authors,
                Tags = tags
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return Created($"/posts/{post.Id}", PostOut.New(post));
        }

        /// <summary>
        /// Update a blog post.
        /// </summary>
        [HttpPatch("{id}")]
        [Authorize(Roles = "Blogger")]
        public async Task<IActionResult> EditPost(int id, EditPostIn dto)
        {
            var userId = User.GetId();

            var post = await _context.Posts.Include(p => p.Authors).FirstOrDefaultAsync(p => p.Id == id);
            if (post is null)
                return NotFound("Post not found.");

            if (!post.Authors.Any(a => a.UserId == userId))
                return BadRequest("You must be one of the post authors to be able to edit it.");

            post.Edit(dto.Title, dto.Resume, dto.Body);

            await _context.SaveChangesAsync();

            return Ok(PostOut.New(post));
        }

        /// <summary>
        /// Comment on a blog post.
        /// </summary>
        [HttpPost("{postId}/comments")]
        [Authorize(Roles = "Reader, Blogger")]
        public async Task<IActionResult> PostComment(int postId, CommentIn dto)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            if (post is null)
                return NotFound("Post not found.");

            var userId = User.GetId();

            var comment = new Comment
            {
                PostId = postId,
                Body = dto.Body,
                CreatedAt = DateTime.Now,
                UserId = userId
            };
            comment.SetPostRating(dto.PostRating);

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return Created($"/posts/{post.Id}/comments/{comment.Id}", CommentOut.New(comment));
        }

        /// <summary>
        /// Pins a comment to a blog post.
        /// </summary>
        [HttpPatch("{postId}/comments/{commentId}/pins")]
        [Authorize(Policy = CommentPinPolicy)]
        public async Task<IActionResult> PostCommentPin(int postId, int commentId)
        {
            var post = await _context.Posts.Include(p => p.Authors).FirstOrDefaultAsync(p => p.Id == postId);
            if (post is null)
                return NotFound("Post not found.");

            var userId = User.GetId();
            if (!post.Authors.Any(a => a.UserId == userId))
                return BadRequest("You must be one of the post authors to be able to pin a comment.");

            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId && c.PostId == post.Id);
            if (comment is null)
                return NotFound("Comment not found.");

            post.PinnedCommentId = (post.PinnedCommentId == comment.Id) ? null : comment.Id;

            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Reply to a comment on a blog post.
        /// </summary>
        [HttpPost("{postId}/comments/{commentId}/replies")]
        [Authorize(Roles = "Reader, Blogger")]
        public async Task<IActionResult> PostCommentReply(int postId, int commentId, ReplyIn dto)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            if (post is null)
                return NotFound("Post not found.");

            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId && c.PostId == post.Id);
            if (comment is null)
                return NotFound("Comment not found.");

            var userId = User.GetId();

            var reply = new Reply
            {
                CommentId = comment.Id,
                Body = dto.Body,
                CreatedAt = DateTime.Now,
                UserId = userId
            };

            _context.Replies.Add(reply);
            await _context.SaveChangesAsync();

            return Created($"/posts/{post.Id}/comments/{comment.Id}/replies/{reply.Id}", ReplyOut.New(reply));
        }

        /// <summary>
        /// Like a comment on a blog post.
        /// </summary>
        [HttpPost("{postId}/comments/{commentId}/likes")]
        [Authorize(Policy = CommentLikePolicy)]
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

            like = new Like
            {
                CommentId = comment.Id,
                CreatedAt = DateTime.Now,
                UserId = userId
            };

            _context.Likes.Add(like);
            await _context.SaveChangesAsync();

            return Created($"/posts/{post.Id}/comments/{comment.Id}/likes/{like.Id}", LikeOut.New(comment.Id, userId));
        }

        /// <summary>
        /// Returns a post.
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<PostOut>> GetPost(int id)
        {
            var post = await _context.Posts
                .AsNoTrackingWithIdentityResolution()
                .Include(p => p.Category)
                .Include(l => l.Authors)
                .Include(l => l.Comments)
                    .ThenInclude(c => c.Replies)
                .Include(l => l.Comments)
                    .ThenInclude(c => c.Likes)
                .Include(l => l.Tags)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (post is null)
                return NotFound("Post not found.");

            return Ok(PostOut.New(post, Request.GetRoot()));
        }

        /// <summary>
        /// Returns all the posts.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<PostOut>>> GetPosts([FromQuery] string? tag)
        {
            var posts = await _context.Posts
                .AsNoTrackingWithIdentityResolution()
                .Include(p => p.Category)
                .Include(l => l.Authors)
                .Include(l => l.Comments)  // To calculate the post rating...
                .Include(l => l.Tags)
                .Where(p => p.Tags.Any(t => t.Name == tag) || string.IsNullOrEmpty(tag))
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return Ok(posts.Select(p => PostOut.NewWithoutComments(p, Request.GetRoot())).ToList());
        }
    }
}
