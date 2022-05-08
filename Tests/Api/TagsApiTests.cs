using System.Net;
using Blog.Controllers.Tags;
using Newtonsoft.Json;
using NUnit.Framework;
using FluentAssertions;

namespace Blog.Tests.Api;

[TestFixture]
public class TagsApiTests : ApiTestBase
{
    [Test]
    public async Task Register_a_new_tag()
    {
        await Login("sam@blog.com", "Test@123");

        var tagIn = new TagIn { Name = "Carrer" };

        var response = await _client.PostAsync("/tags", tagIn.ToStringContent());

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Test]
    public async Task Get_a_tag()
    {
        var id = 1;
        var name = "Tech";
        var postsCount = 2;
        var response = await _client.GetAsync($"/tags/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tag = JsonConvert.DeserializeObject<TagOut>(await response.Content.ReadAsStringAsync());

        tag.Name.Should().Be(name);
        tag.CreatedAt.Should().NotBeNullOrEmpty();
        tag.Posts.Count.Should().Be(postsCount);
    }

    [Test]
    public async Task Get_all_tags()
    {
        var response = await _client.GetAsync("/tags/?pageSize=3");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tags = JsonConvert.DeserializeObject<List<TagOut>>(await response.Content.ReadAsStringAsync());

        tags.Count.Should().Be(3);
    }
}
