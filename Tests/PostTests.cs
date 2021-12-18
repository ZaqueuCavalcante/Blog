using Blog.Domain;
using Blog.Exceptions;
using NUnit.Framework;
using Shouldly;

namespace Blog.Tests
{
    [TestFixture]
    public class PostTests
    {
        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(6)]
        public void Post_rating_out_of_range(int postRating)
        {
            var comment = new Comment{};

            var exception = Should.Throw<DomainException>(() => comment.SetPostRating((byte) postRating));

            exception.Message.ShouldBe("Post rating out of range. Must be between 1 and 5.");
        }

        [Test]
        public void Post_get_rating_without_comments()
        {
            var post = new Post{};

            var postRating = post.GetRating();

            postRating.ShouldBe((byte) 0);
        }

        [Test]
        public void Post_get_rating_with_comments()
        {
            var post = new Post{};

            var comments = new List<Comment>
            {
                new Comment{}, new Comment{}, new Comment{}
            };

            byte postRating = 1;
            comments.ForEach(c => c.SetPostRating(postRating++));

            post.Comments = comments;

            post.GetRating().ShouldBe((byte) 2);
        }
    }
}
