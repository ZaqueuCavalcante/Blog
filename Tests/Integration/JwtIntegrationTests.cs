using Blog.Domain;
using Blog.Exceptions;
using NUnit.Framework;
using Shouldly;

namespace Blog.Tests.Unit;

[TestFixture]
public class JwtIntegrationTests
{


    [Test]
    public void Post_get_rating_without_comments()
    {
        var post = new Post(
            title: "Linux and hacking",
            resume: "Linux Basics for Hackers: Getting Started with Networking, Scripting, and Security in Kali.",
            body: "This practical, tutorial-style book uses the Kali Linux distribution to teach Linux basics with a focus on how hackers would use them.",
            categoryId: 1,
            authorId: 1  
        );

        var postRating = post.GetRating();

        postRating.ShouldBe((byte) 0);
    }
}
