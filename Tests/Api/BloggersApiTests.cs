using System.Net;
using Blog.Controllers.Bloggers;
using Newtonsoft.Json;
using NUnit.Framework;
using FluentAssertions;

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

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
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

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Test]
    public async Task Get_a_blogger()
    {
        var id = 2;
        var name = "Elliot Alderson";
        var resume = "Writes about Linux, Hacking and Computers.";

        var response = await _client.GetAsync($"/bloggers/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var blogger = JsonConvert.DeserializeObject<BloggerOut>(await response.Content.ReadAsStringAsync());

        blogger.Id.Should().Be(id);
        blogger.Name.Should().Be(name);
        blogger.Resume.Should().Be(resume);
    }

    [Test]
    public async Task Try_get_a_non_existent_blogger()
    {
        var bloggerId = 42; 
        var response = await _client.GetAsync($"/bloggers/{bloggerId}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Get_all_bloggers()
    {
        var response = await _client.GetAsync("/bloggers");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var bloggers = JsonConvert.DeserializeObject<List<BloggerOut>>(await response.Content.ReadAsStringAsync());

        bloggers.Count.Should().Be(3);
    }

    [Test]
    public async Task Update_a_blogger_data()
    {
        await Login("elliot@blog.com", "Test@123");

        var responseBefore = await _client.GetAsync($"/bloggers/2");
        responseBefore.StatusCode.Should().Be(HttpStatusCode.OK);
        var bloggerBefore = JsonConvert.DeserializeObject<BloggerOut>(await responseBefore.Content.ReadAsStringAsync());
        bloggerBefore.Id.Should().Be(2);
        bloggerBefore.Name.Should().Be("Elliot Alderson");
        bloggerBefore.Resume.Should().Be("Writes about Linux, Hacking and Computers.");

        var bloggerUpdateIn = new BloggerUpdateIn
        {
            Name = "Zaqueu C.",
            Resume = "A .Net Core Blogger..."
        };
        var response = await _client.PatchAsync("/bloggers", bloggerUpdateIn.ToStringContent());
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var responseAfter = await _client.GetAsync($"/bloggers/2");
        responseAfter.StatusCode.Should().Be(HttpStatusCode.OK);
        var bloggerAfter = JsonConvert.DeserializeObject<BloggerOut>(await responseAfter.Content.ReadAsStringAsync());
        bloggerAfter.Id.Should().Be(2);
        bloggerAfter.Name.Should().Be("Zaqueu C.");
        bloggerAfter.Resume.Should().Be("A .Net Core Blogger...");
    }
}
