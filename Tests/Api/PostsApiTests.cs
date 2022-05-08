using System.Net;
using Blog.Controllers.Posts;
using Newtonsoft.Json;
using NUnit.Framework;
using FluentAssertions;

namespace Blog.Tests.Api;

[TestFixture]
public class PostsApiTests : ApiTestBase
{
    [Test]
    public async Task Try_create_a_new_post_without_authorization()
    {
        var postIn = new PostIn
        {
            Title = "A nex blog post",
            Resume = "A resume of the new blog post...",
            Body = "The body of the new blog post...",
            CategoryId = 1,
            Tags = new List<int>{ 1 }
        };

        var response = await _client.PostAsync("/posts", postIn.ToStringContent());

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Create_a_new_post()
    {
        await Login("elliot@blog.com", "Test@123");

        var postIn = new PostIn
        {
            Title = "A new bolg post",
            Resume = "Linux Basics for Hackers: Getting Started with Networking, Scripting, and Security in Kali.",
            Body = "This practical, tutorial-style book uses the Kali Linux distribution to teach Linux basics with a focus on how hackers would use them. Topics include Linux command line basics, filesystems, networking, BASH basics, package management, logging, and the Linux kernel and drivers.",
            CategoryId = 1,
            Tags = new List<int>{ 1 }
        };

        var response = await _client.PostAsync("/posts", postIn.ToStringContent());
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var post = JsonConvert.DeserializeObject<PostOut>(await response.Content.ReadAsStringAsync());

        post.Title.Should().Be(postIn.Title);
        post.Resume.Should().Be(postIn.Resume);
        post.Body.Should().Be(postIn.Body);
        post.Author.Name.Should().Be("Elliot Alderson");
        post.Tags.Should().Contain(t => t.Name == "Tech");
    }

    [Test]
    [TestCase("elliot@blog.com", 2)]
    [TestCase("darlene@blog.com", 4)]
    public async Task Create_a_new_comment(string email, int userId)
    {
        await Login(email, "Test@123");

        var commentIn = new CommentIn
        {
            Body = "A new comment about the post...",
            PostRating = 5
        };

        var response = await _client.PostAsync("/posts/1/comments", commentIn.ToStringContent());
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var comment = JsonConvert.DeserializeObject<CommentOut>(await response.Content.ReadAsStringAsync());

        comment.UserId.Should().Be(userId);
        comment.Body.Should().Be(commentIn.Body);
        comment.PostRating.Should().Be(commentIn.PostRating);
        comment.Likes.Should().Be(0);
        comment.CreatedAt.Should().NotBeNullOrEmpty();
    }

    [Test]
    public async Task Pin_a_comment()
    {
        await Login("elliot@blog.com", "Test@123");

        var postId = 2;
        var commentId = 4;

        var postBeforeResponse = await _client.GetAsync($"/posts/{postId}");
        postBeforeResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var postBefore = JsonConvert.DeserializeObject<PostOut>(await postBeforeResponse.Content.ReadAsStringAsync());
        postBefore.PinnedCommentId.Should().Be(null);

        var response = await _client.PatchAsync($"/posts/{postId}/comments/{commentId}/pins", null);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var postAfterResponse = await _client.GetAsync($"/posts/{postId}");
        postAfterResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var postAfter = JsonConvert.DeserializeObject<PostOut>(await postAfterResponse.Content.ReadAsStringAsync());
        postAfter.PinnedCommentId.Should().Be(commentId);
    }

    [Test]
    public async Task Unpin_a_comment()
    {
        await Login("elliot@blog.com", "Test@123");
        
        var postId = 2;
        var commentId = 4;

        await _client.PatchAsync($"/posts/{postId}/comments/{commentId}/pins", null);

        var response = await _client.PatchAsync($"/posts/{postId}/comments/{commentId}/pins", null);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var postAfterResponse = await _client.GetAsync($"/posts/{postId}");
        postAfterResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var postAfter = JsonConvert.DeserializeObject<PostOut>(await postAfterResponse.Content.ReadAsStringAsync());
        postAfter.PinnedCommentId.Should().Be(null);
    }

    [Test]
    [TestCase("elliot@blog.com", 2)]
    [TestCase("darlene@blog.com", 4)]
    public async Task Create_a_new_comment_reply(string email, int userId)
    {
        await Login(email, "Test@123");

        var postId = 1;
        var commentId = 3;
        var replyIn = new ReplyIn
        {
            Body = "A new comment reply..."
        };

        var response = await _client.PostAsync($"/posts/{postId}/comments/{commentId}/replies", replyIn.ToStringContent());
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var reply = JsonConvert.DeserializeObject<ReplyOut>(await response.Content.ReadAsStringAsync());

        reply.UserId.Should().Be(userId);
        reply.Body.Should().Be(replyIn.Body);
        reply.CreatedAt.Should().NotBeNullOrEmpty();
    }

    [Test]
    [TestCase("irving@blog.com", 3)]
    [TestCase("darlene@blog.com", 4)]
    public async Task Like_a_comment(string email, int userId)
    {
        await Login(email, "Test@123");

        var postId = 1;
        var commentId = 2;

        var response = await _client.PostAsync($"/posts/{postId}/comments/{commentId}/likes", null);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var like = JsonConvert.DeserializeObject<LikeOut>(await response.Content.ReadAsStringAsync());

        like.UserId.Should().Be(userId);
        like.CommentId.Should().Be(commentId);
    }

    [Test]
    [TestCase(1, "Mr. Robot - End explained", (byte) 4)]
    [TestCase(2, "Linux and hacking", (byte) 5)]
    public async Task Get_a_post(int id, string title, byte rating)
    {
        var response = await _client.GetAsync($"/posts/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var post = JsonConvert.DeserializeObject<PostOut>(await response.Content.ReadAsStringAsync());

        post.Id.Should().Be(id);
        post.Title.Should().Be(title);
        post.Rating.Should().Be(rating);
    }

    [Test]
    public async Task Get_all_posts()
    {
        var response = await _client.GetAsync("/posts/?pageSize=3");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var posts = JsonConvert.DeserializeObject<List<PostOut>>(await response.Content.ReadAsStringAsync());

        posts.Count.Should().Be(3);
    }
}
