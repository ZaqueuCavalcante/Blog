using System.Net;
using Blog.Controllers.Categories;
using Newtonsoft.Json;
using NUnit.Framework;
using Shouldly;

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

        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }

    [Test]
    public async Task Get_a_category()
    {
        var id = 1;
        var name = "Linux";
        var description = "The Linux category description.";
        var response = await _client.GetAsync($"/categories/{id}");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var category = JsonConvert.DeserializeObject<CategoryOut>(await response.Content.ReadAsStringAsync());

        category.Name.ShouldBe(name);
        category.Description.ShouldBe(description);
        category.CreatedAt.ShouldNotBeNullOrEmpty();
        category.Posts.Count.ShouldBe(1);
    }

    [Test]
    public async Task Get_all_categories()
    {
        var response = await _client.GetAsync("/categories/?pageSize=5");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var categories = JsonConvert.DeserializeObject<List<CategoryOut>>(await response.Content.ReadAsStringAsync());

        categories.Count.ShouldBe(5);
        categories.ShouldContain(c => c.Name == "Linux");
        categories.ShouldContain(c => c.Name == "Mr. Robot");
    }
}
