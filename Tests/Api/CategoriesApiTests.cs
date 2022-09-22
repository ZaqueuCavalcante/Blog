using System.Net;
using Blog.Controllers.Categories;
using Newtonsoft.Json;
using NUnit.Framework;
using FluentAssertions;

namespace Blog.Tests.Api;

[TestFixture]
public class CategoriesApiTests : ApiTestBase
{
    [Test]
    public async Task Register_a_new_category()
    {
        await Login("sam@blog.com", "Test@123");

        var categoryIn = new CategoryIn
        {
            Name = "Identity Server",
            Description = "Things about auth in ASP.NET Core."
        };

        var response = await _client.PostAsync("/categories", categoryIn.ToStringContent());

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Test]
    public async Task Get_a_category()
    {
        var id = 1;
        var name = "Linux";
        var description = "The Linux category description.";
        var response = await _client.GetAsync($"/categories/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var category = await response.DeserializeTo<CategoryOut>();

        category.Name.Should().Be(name);
        category.Description.Should().Be(description);
        category.CreatedAt.Should().NotBeNullOrEmpty();
    }

    [Test]
    public async Task Get_all_categories()
    {
        var response = await _client.GetAsync("/categories/?pageSize=5");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var categories = await response.DeserializeTo<List<CategoryOut>>();

        categories.Count.Should().Be(5);
        categories.Should().Contain(c => c.Name == "Linux");
        categories.Should().Contain(c => c.Name == "Mr. Robot");
    }
}
