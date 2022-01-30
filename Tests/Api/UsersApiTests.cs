using System.Net;
using Blog.Controllers.Users;
using Newtonsoft.Json;
using NUnit.Framework;
using Shouldly;

namespace Blog.Tests.Api;

[TestFixture]
public class UsersApiTests : ApiTestBase
{
    [Test]
    public async Task Login_into_blog()
    {
        var userIn = new UserIn { Email = "sam@blog.com", Password = "Test@123" };
        var loginResponse = await _client.PostAsync("users/login", userIn.ToStringContent());
        loginResponse.StatusCode.ShouldBe(HttpStatusCode.OK);

        var loginOut = JsonConvert.DeserializeObject<LoginOut>(await loginResponse.Content.ReadAsStringAsync());

        loginOut.AccessToken.ShouldNotBeNullOrEmpty();
        loginOut.ExpiresInMinutes.ShouldBe("5");
        loginOut.RefreshToken.ShouldNotBeNullOrEmpty();
        loginOut.Scope.ShouldBe("create");
        loginOut.TokenType.ShouldBe("Bearer");
    }
}
