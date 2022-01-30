using Blog.Domain;
using Blog.Exceptions;
using NUnit.Framework;
using Shouldly;

namespace Blog.Tests.Unit;

[TestFixture]
public class PostsUnitTests
{
    [Test]
    [TestCase(-1)]
    [TestCase(0)]
    [TestCase(6)]
    public void Post_rating_out_of_range(int postRating)
    {
        var exception = Should.Throw<DomainException>(() =>
            new Comment(1, (byte) postRating, "Great first season.", 1)
        );

        exception.Message.ShouldBe("Post rating out of range. Must be between 1 and 5.");
    }

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
