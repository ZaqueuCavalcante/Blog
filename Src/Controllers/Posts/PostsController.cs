﻿using System.Security.Claims;
using Blog.Database;
using Blog.Domain;
using Blog.Filters;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Blogger")]
        public async Task<IActionResult> PostPost(PostIn dto)
        {
            var userId = int.Parse(User.FindFirstValue("sub"));
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

            return Created($"/posts/{post.Id}", new PostOut(post));
        }

        [HttpPost("{postId}/comments")]
        [Authorize(Roles = "Reader, Blogger")]
        public async Task<IActionResult> PostComment(int postId, CommentIn dto)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            if (post is null)
                return NotFound("Post not found.");

            var userId = int.Parse(User.FindFirstValue("sub"));

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

            return Created($"/posts/{post.Id}", new PostOut(post));
        }

        [HttpPut("{postId}/comments/{commentId}/pins")]
        [Authorize(Roles = "Blogger")]
        [ClaimsAuthorize("pinner", "true")]
        public async Task<IActionResult> PostCommentPin(int postId, int commentId)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            if (post is null)
                return NotFound("Post not found.");

            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId && c.PostId == post.Id);
            if (comment is null)
                return NotFound("Comment not found.");

            post.PinnedCommentId = (post.PinnedCommentId == comment.Id) ? null : comment.Id;

            await _context.SaveChangesAsync();

            return Ok();
        }

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

            var userId = int.Parse(User.FindFirstValue("sub"));

            var reply = new Reply
            {
                CommentId = comment.Id,
                Body = dto.Body,
                CreatedAt = DateTime.Now,
                UserId = userId
            };

            _context.Replies.Add(reply);
            await _context.SaveChangesAsync();

            return Created($"/posts/{post.Id}", new PostOut(post));
        }

        [HttpPost("{postId}/comments/{commentId}/likes")]
        [Authorize(Roles = "Reader, Blogger")]
        public async Task<IActionResult> PostCommentLike(int postId, int commentId)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            if (post is null)
                return NotFound("Post not found.");

            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId && c.PostId == post.Id);
            if (comment is null)
                return NotFound("Comment not found.");

            var userId = int.Parse(User.FindFirstValue("sub"));

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

            return Ok("Like added.");
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<PostOut>> GetPost(int id)
        {
            var post = await _context.Posts
                .AsNoTrackingWithIdentityResolution()
                .Include(l => l.Authors)
                .Include(l => l.Comments)
                    .ThenInclude(c => c.Replies)
                .Include(l => l.Comments)
                    .ThenInclude(c => c.Likes)
                .Include(l => l.Tags)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (post is null)
                return NotFound("Post not found.");

            return Ok(new PostOut(post));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<PostOut>>> GetPosts([FromQuery] string? tag)
        {
            var posts = await _context.Posts
                .AsNoTrackingWithIdentityResolution()
                .Include(l => l.Authors)
                .Include(l => l.Comments)
                    .ThenInclude(c => c.Replies)
                .Include(l => l.Comments)
                    .ThenInclude(c => c.Likes)
                .Include(l => l.Tags)
                .Where(p => p.Tags.Any(t => t.Name == tag) || string.IsNullOrEmpty(tag))
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return Ok(posts.Select(x => new PostOut(x)).ToList());
        }
    }
}
