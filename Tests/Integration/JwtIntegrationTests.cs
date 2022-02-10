using Blog.Auth;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Shouldly;

namespace Blog.Tests.Integration;

[TestFixture]
public class JwtIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Post_get_rating_without_comments()
    {
        using var scope = _factory.Services.CreateScope();
        _tokenManager = scope.ServiceProvider.GetRequiredService<TokenManager>();

        var userEmail = "sam@blog.com";
        var (accessToken, refreshToken) = await _tokenManager.GenerateTokens(userEmail);

        accessToken.ShouldNotBeNullOrEmpty();
        refreshToken.ShouldNotBeNullOrEmpty();
    }
}
