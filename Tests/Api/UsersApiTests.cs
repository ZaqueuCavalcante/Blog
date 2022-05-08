using System.Net;
using Blog.Controllers.Users;
using Newtonsoft.Json;
using NUnit.Framework;
using FluentAssertions;

namespace Blog.Tests.Api;

[TestFixture]
public class UsersApiTests : ApiTestBase
{
    [Test]
    public async Task Login_into_blog()
    {
        var userIn = new UserIn { Email = "sam@blog.com", Password = "Test@123" };
        var loginResponse = await _client.PostAsync("users/login", userIn.ToStringContent());
        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var loginOut = JsonConvert.DeserializeObject<LoginOut>(await loginResponse.Content.ReadAsStringAsync());

        loginOut.AccessToken.Should().NotBeNullOrEmpty();
        loginOut.ExpiresInMinutes.Should().Be("5");
        loginOut.RefreshToken.Should().NotBeNullOrEmpty();
        loginOut.Scope.Should().Be("create");
        loginOut.TokenType.Should().Be("Bearer");
    }
}
