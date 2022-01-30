using System.Net;
using Blog.Controllers.Bloggers;
using Newtonsoft.Json;
using NUnit.Framework;
using Shouldly;

namespace Blog.Tests.Api;

[TestFixture]
public class BloggersApiTests : ApiTestBase
{
    [Test]
    public async Task Try_register_a_new_blogger_without_authorization()
    {
        var bloggerIn = new BloggerIn
        {
            Name = "Zaqueu C.",
            Resume = "A .Net Core Blogger...",
            Email = "zaqueu@blog.com",
            Password = "Test@123"
        };

        var response = await _client.PostAsync("/bloggers", bloggerIn.ToStringContent());

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Register_a_new_blogger()
    {
        await Login("sam@blog.com", "Test@123");

        var bloggerIn = new BloggerIn
        {
            Name = "Zaqueu C.",
            Resume = "A .Net Core Blogger...",
            Email = "zaqueu@blog.com",
            Password = "Test@123"
        };

        var response = await _client.PostAsync("/bloggers", bloggerIn.ToStringContent());

        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }

    [Test]
    public async Task Get_a_blogger()
    {
        var id = 2;
        var name = "Elliot Alderson";
        var resume = "Writes about Linux, Hacking and Computers.";

        var response = await _client.GetAsync($"/bloggers/{id}");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var blogger = JsonConvert.DeserializeObject<BloggerOut>(await response.Content.ReadAsStringAsync());

        blogger.Id.ShouldBe(id);
        blogger.Name.ShouldBe(name);
        blogger.Resume.ShouldBe(resume);
    }

    [Test]
    public async Task Try_get_a_non_existent_blogger()
    {
        var bloggerId = 42; 
        var response = await _client.GetAsync($"/bloggers/{bloggerId}");
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Get_all_bloggers()
    {
        var response = await _client.GetAsync("/bloggers");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var bloggers = JsonConvert.DeserializeObject<List<BloggerOut>>(await response.Content.ReadAsStringAsync());

        bloggers.Count.ShouldBe(3);
    }

    [Test]
    public async Task Update_a_blogger_data()
    {
        await Login("elliot@blog.com", "Test@123");

        var responseBefore = await _client.GetAsync($"/bloggers/2");
        responseBefore.StatusCode.ShouldBe(HttpStatusCode.OK);
        var bloggerBefore = JsonConvert.DeserializeObject<BloggerOut>(await responseBefore.Content.ReadAsStringAsync());
        bloggerBefore.Id.ShouldBe(2);
        bloggerBefore.Name.ShouldBe("Elliot Alderson");
        bloggerBefore.Resume.ShouldBe("Writes about Linux, Hacking and Computers.");

        var bloggerUpdateIn = new BloggerUpdateIn
        {
            Name = "Zaqueu C.",
            Resume = "A .Net Core Blogger..."
        };
        var response = await _client.PatchAsync("/bloggers", bloggerUpdateIn.ToStringContent());
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var responseAfter = await _client.GetAsync($"/bloggers/2");
        responseAfter.StatusCode.ShouldBe(HttpStatusCode.OK);
        var bloggerAfter = JsonConvert.DeserializeObject<BloggerOut>(await responseAfter.Content.ReadAsStringAsync());
        bloggerAfter.Id.ShouldBe(2);
        bloggerAfter.Name.ShouldBe("Zaqueu C.");
        bloggerAfter.Resume.ShouldBe("A .Net Core Blogger...");
    }

    [Test]
    public async Task Get_blogger_stats()
    {
        await Login("elliot@blog.com", "Test@123");

        var response = await _client.GetAsync("/bloggers/stats");
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var stats = JsonConvert.DeserializeObject<BloggerStatsOut>(await response.Content.ReadAsStringAsync());

        stats.PublishedPosts.ShouldBe(1);
        stats.DraftPosts.ShouldBe(0);
        stats.LatestComments.Count.ShouldBe(2);
    }
}
