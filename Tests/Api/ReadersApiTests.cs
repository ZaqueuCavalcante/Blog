using System.Net;
using Blog.Controllers.Readers;
using Newtonsoft.Json;
using NUnit.Framework;
using Shouldly;

namespace Blog.Tests.Api;

[TestFixture]
public class ReadersApiTests : ApiTestBase
{
    [Test]
    public async Task Register_a_new_reader()
    {
        var readerIn = new ReaderIn
        {
            Name = "Zaqueu C.",
            Email = "zaqueu@blog.com",
            Password = "Test@123"
        };

        var response = await _client.PostAsync("/readers", readerIn.ToStringContent());

        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var reader = JsonConvert.DeserializeObject<ReaderOut>(await response.Content.ReadAsStringAsync());

        reader.Name.ShouldBe(readerIn.Name);
    }

    [Test]
    public async Task Get_a_reader()
    {
        var id = 1;
        var name = "Darlene Alderson";
        var response = await _client.GetAsync($"/readers/{id}");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var reader = JsonConvert.DeserializeObject<ReaderOut>(await response.Content.ReadAsStringAsync());

        reader.Id.ShouldBe(id);
        reader.Name.ShouldBe(name);
    }

    [Test]
    public async Task Get_all_readers()
    {
        var response = await _client.GetAsync("/readers/?pageSize=4");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var readers = JsonConvert.DeserializeObject<List<ReaderOut>>(await response.Content.ReadAsStringAsync());

        readers.Count.ShouldBe(4);
    }
}
