using System.Net;
using System.Net.Http.Headers;
using Blog.Controllers.Bloggers;
using Blog.Controllers.Categories;
using Blog.Controllers.Posts;
using Blog.Controllers.Readers;
using Blog.Controllers.Tags;
using Blog.Controllers.Users;
using Newtonsoft.Json;
using NUnit.Framework;
using Shouldly;

namespace Blog.Tests
{
    [TestFixture]
    public class ApiTests
    {
        private HttpClient _client;
        private APIWebApplicationFactory _factory;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _factory = new APIWebApplicationFactory();
        }

        [SetUp]
        public async Task SetupBeforeEachTest()
        {
            _client = _factory.CreateClient();
            await _client.GetAsync("/seed");  // TODO: refactor this
        }

        #region Users

        private async Task Login(string email, string password)
        {
            var userIn = new UserIn { Email = email, Password = password };
            var loginResponse = await _client.PostAsync("users/login", userIn.ToStringContent());
            loginResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
            var loginOut = JsonConvert.DeserializeObject<LoginOut>(await loginResponse.Content.ReadAsStringAsync());
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginOut.AccessToken);    
        }

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

        #endregion
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        #region Bloggers

        [Test]
        public async Task Try_register_a_new_blogger_without_authorization()
        {
            var bloggerIn = new BloggerIn
            {
                Name = "Zaqueu C.",
                Resume = "A .Net Core Blogger...",
                Email = "zaqueu@blog.com",
                Password = "Test@123"
            };

            var response = await _client.PostAsync("/bloggers", bloggerIn.ToStringContent());

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Test]
        public async Task Register_a_new_blogger()
        {
            await Login("sam@blog.com", "Test@123");

            var bloggerIn = new BloggerIn
            {
                Name = "Zaqueu C.",
                Resume = "A .Net Core Blogger...",
                Email = "zaqueu@blog.com",
                Password = "Test@123"
            };

            var response = await _client.PostAsync("/bloggers", bloggerIn.ToStringContent());

            response.StatusCode.ShouldBe(HttpStatusCode.Created);
        }

        [Test]
        [TestCase(1, "Sam Esmail", "Writes about ASP.NET Core, DevOps and TV Shows.")]
        public async Task Get_a_blogger(int id, string name, string resume)
        {
            var response = await _client.GetAsync($"/bloggers/{id}");

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var blogger = JsonConvert.DeserializeObject<BloggerOut>(await response.Content.ReadAsStringAsync());

            blogger.Id.ShouldBe(id);
            blogger.Name.ShouldBe(name);
            blogger.Resume.ShouldBe(resume);
        }

        [Test]
        public async Task Try_get_a_non_existent_blogger()
        {
            var bloggerId = 42; 
            var response = await _client.GetAsync($"/bloggers/{bloggerId}");
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Test]
        public async Task Get_all_bloggers()
        {
            var response = await _client.GetAsync("/bloggers");

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var bloggers = JsonConvert.DeserializeObject<List<BloggerOut>>(await response.Content.ReadAsStringAsync());

            bloggers.Count.ShouldBe(3);
        }

        [Test]
        public async Task Update_a_blogger_data()
        {
            await Login("elliot@blog.com", "Test@123");

            var responseBefore = await _client.GetAsync($"/bloggers/2");
            responseBefore.StatusCode.ShouldBe(HttpStatusCode.OK);
            var bloggerBefore = JsonConvert.DeserializeObject<BloggerOut>(await responseBefore.Content.ReadAsStringAsync());
            bloggerBefore.Id.ShouldBe(2);
            bloggerBefore.Name.ShouldBe("Elliot Alderson");
            bloggerBefore.Resume.ShouldBe("Writes about Linux, Hacking and Computers.");


            var bloggerUpdateIn = new BloggerUpdateIn
            {
                Name = "Zaqueu C.",
                Resume = "A .Net Core Blogger..."
            };
            var response = await _client.PatchAsync("/bloggers", bloggerUpdateIn.ToStringContent());
            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);


            var responseAfter = await _client.GetAsync($"/bloggers/2");
            responseAfter.StatusCode.ShouldBe(HttpStatusCode.OK);
            var bloggerAfter = JsonConvert.DeserializeObject<BloggerOut>(await responseAfter.Content.ReadAsStringAsync());
            bloggerAfter.Id.ShouldBe(2);
            bloggerAfter.Name.ShouldBe("Zaqueu C.");
            bloggerAfter.Resume.ShouldBe("A .Net Core Blogger...");
        }

        [Test]
        public async Task Get_blogger_stats()
        {
            await Login("elliot@blog.com", "Test@123");

            var response = await _client.GetAsync("/bloggers/stats");
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var stats = JsonConvert.DeserializeObject<BloggerStatsOut>(await response.Content.ReadAsStringAsync());

            stats.PublishedPosts.ShouldBe(1);
            stats.DraftPosts.ShouldBe(0);
            stats.LatestComments.Count.ShouldBe(2);
        }

        #endregion
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        #region Categories

        [Test]
        [TestCase(1, "Linux", "The Linux category description.")]
        public async Task Get_a_category(int id, string name, string description)
        {
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

        #endregion
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        #region Posts

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

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
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
            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            var post = JsonConvert.DeserializeObject<PostOut>(await response.Content.ReadAsStringAsync());

            post.Title.ShouldBe(postIn.Title);
            post.Resume.ShouldBe(postIn.Resume);
            post.Body.ShouldBe(postIn.Body);
            post.Author.Name.ShouldBe("Elliot Alderson");
            post.Tags.ShouldContain(t => t.Name == "Tech");
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
            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            var comment = JsonConvert.DeserializeObject<CommentOut>(await response.Content.ReadAsStringAsync());

            comment.UserId.ShouldBe(userId);
            comment.Body.ShouldBe(commentIn.Body);
            comment.PostRating.ShouldBe(commentIn.PostRating);
            comment.Likes.ShouldBe(0);
            comment.CreatedAt.ShouldNotBeNullOrEmpty();
        }

        [Test]
        public async Task Pin_a_comment()
        {
            await Login("elliot@blog.com", "Test@123");

            var postId = 2;
            var commentId = 4;

            var postBeforeResponse = await _client.GetAsync($"/posts/{postId}");
            postBeforeResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
            var postBefore = JsonConvert.DeserializeObject<PostOut>(await postBeforeResponse.Content.ReadAsStringAsync());
            postBefore.PinnedCommentId.ShouldBe(null);

            var response = await _client.PatchAsync($"/posts/{postId}/comments/{commentId}/pins", null);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var postAfterResponse = await _client.GetAsync($"/posts/{postId}");
            postAfterResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
            var postAfter = JsonConvert.DeserializeObject<PostOut>(await postAfterResponse.Content.ReadAsStringAsync());
            postAfter.PinnedCommentId.ShouldBe(commentId);
        }

        [Test]
        public async Task Unpin_a_comment()
        {
            await Pin_a_comment();
            
            var postId = 2;
            var commentId = 4;

            var response = await _client.PatchAsync($"/posts/{postId}/comments/{commentId}/pins", null);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var postAfterResponse = await _client.GetAsync($"/posts/{postId}");
            postAfterResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
            var postAfter = JsonConvert.DeserializeObject<PostOut>(await postAfterResponse.Content.ReadAsStringAsync());
            postAfter.PinnedCommentId.ShouldBe(null);
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
            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            var reply = JsonConvert.DeserializeObject<ReplyOut>(await response.Content.ReadAsStringAsync());

            reply.UserId.ShouldBe(userId);
            reply.Body.ShouldBe(replyIn.Body);
            reply.CreatedAt.ShouldNotBeNullOrEmpty();
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
            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            var like = JsonConvert.DeserializeObject<LikeOut>(await response.Content.ReadAsStringAsync());

            like.UserId.ShouldBe(userId);
            like.CommentId.ShouldBe(commentId);
        }

        [Test]
        [TestCase(1, "Mr. Robot - End explained", (byte) 4)]
        [TestCase(2, "Linux and hacking", (byte) 5)]
        public async Task Get_a_post(int id, string title, byte rating)
        {
            var response = await _client.GetAsync($"/posts/{id}");

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var post = JsonConvert.DeserializeObject<PostOut>(await response.Content.ReadAsStringAsync());

            post.Id.ShouldBe(id);
            post.Title.ShouldBe(title);
            post.Rating.ShouldBe(rating);
        }

        [Test]
        public async Task Get_all_posts()
        {
            var response = await _client.GetAsync("/posts/?pageSize=3");

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var posts = JsonConvert.DeserializeObject<List<PostOut>>(await response.Content.ReadAsStringAsync());

            posts.Count.ShouldBe(3);
        }

        #endregion
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        #region Readers

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
        [TestCase(1, "Darlene Alderson")]
        [TestCase(2, "Tyrell Wellick")]
        public async Task Get_a_reader(int id, string name)
        {
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

        #endregion
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        #region Tags

        [Test]
        [TestCase(1, "Tech", 2)]
        [TestCase(2, "Series", 1)]
        [TestCase(3, "Hacking", 2)]
        public async Task Get_a_tag(int id, string name, int posts)
        {
            var response = await _client.GetAsync($"/tags/{id}");

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var tag = JsonConvert.DeserializeObject<TagOut>(await response.Content.ReadAsStringAsync());

            tag.Name.ShouldBe(name);
            tag.CreatedAt.ShouldNotBeNullOrEmpty();
            tag.Posts.Count.ShouldBe(posts);
        }

        [Test]
        public async Task Get_all_tags()
        {
            var response = await _client.GetAsync("/tags/?pageSize=3");

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var tags = JsonConvert.DeserializeObject<List<TagOut>>(await response.Content.ReadAsStringAsync());

            tags.Count.ShouldBe(3);
        }

        #endregion
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    }
}
