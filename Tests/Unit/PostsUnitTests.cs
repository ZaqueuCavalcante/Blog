using Blog.Domain;
using Blog.Exceptions;
using FluentAssertions;
using NUnit.Framework;

namespace Blog.Tests.Unit;

[TestFixture]
public class PostsUnitTests
{
    [Teste("Para comentar em um post, a nota de avalição deve ser válida.")]
    [TestCaseSource(typeof(Streams), nameof(Streams.InvalidPostRatingsStream))]
    public void PostsUnitTests_00(int postRating)
    {
        // Arrange
        Action act = () => new Comment(1, (byte) postRating, "Great first season.", 1);

        // Act / Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Post rating out of range. Must be between 1 and 5.");
    }

    [Teste("Um post sem comentários não deve possuir nota de avaliação.")]
    public void PostsUnitTests_01()
    {
        // Arrange
        var post = new Post(
            title: "Linux and hacking",
            resume: "Linux Basics for Hackers: Getting Started with Networking, Scripting, and Security in Kali.",
            body: "This practical, tutorial-style book uses the Kali Linux distribution to teach Linux basics with a focus on how hackers would use them.",
            categoryId: 1,
            authorId: 1  
        );

        // Act
        var postRating = post.GetRating();

        // Assert
        postRating.Should().Be((byte) 0);
    }
}
