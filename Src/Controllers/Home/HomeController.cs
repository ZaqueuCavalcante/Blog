﻿using Blog.Extensions;
using Blog.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers.Home;

[ApiController, Route("")]
public class HomeController : ControllerBase
{
    private readonly BlogContext _context;

    public HomeController(
        BlogContext context
    ) {
        _context = context;
    }

    /// <summary>
    /// Returns all informations about the blog home page.
    /// </summary>
    [HttpGet, AllowAnonymous]
    public async Task<ActionResult> GetHome([FromQuery] HomeParameters parameters)
    {
        var url = Request.GetRoot();

        var lastPosts = await _context.Posts
            .OrderByDescending(p => p.CreatedAt)
            .Page(parameters)
            .ToListAsync();
        var lastPostsOut = lastPosts.Select(p => new HomePostOut(p, url)).ToList();

        var count = await _context.Posts.CountAsync();
        Response.AddPagination(parameters, count);

        var bloggers = await _context.Bloggers
            .Include(b => b.Networks)
            .ToListAsync();
        var bloggersOut = new List<HomeBloggerOut>();
        bloggers.ForEach(b => bloggersOut.Add(new HomeBloggerOut(b, url)));

        var categories = await _context.Categories
            .Include(c => c.Posts)
            .ToListAsync();
        var categoriesOut = categories.Select(c => new HomeCategoryOut(c, url));

        var tags = await _context.Tags
            .ToListAsync();
        var tagsOut = tags.Select(t => new HomeTagOut(t, url));

        var response = new
        {
            Links = new
            {
                Home = url,
                Search = url + "search",
                Bloggers = url + "bloggers",
                Posts = url + "posts",
                Categories = url + "categories",
                Tags = url + "tags",
                Login = url + "users/login"
            },
            LastPosts = lastPostsOut,
            Bloggers = bloggersOut,
            Categories = categoriesOut,
            Tags = tagsOut
        };

        return Ok(response);
    }
}
