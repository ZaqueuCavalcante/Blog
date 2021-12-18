using System.Net;
using System.Net.Http.Headers;
using Blog.Controllers.Bloggers;
using Blog.Controllers.Categories;
using Blog.Controllers.Posts;
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

        private async Task Login(string email, string password)
        {
            var userIn = new UserIn
            {
                Email = email,
                Password = password
            };
            var loginResponse = await _client.PostAsync("users/login", userIn.ToStringContent());
            loginResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
            var loginOut = JsonConvert.DeserializeObject<LoginOut>(await loginResponse.Content.ReadAsStringAsync());
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginOut.AccessToken);    
        }

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
        [TestCase(2, "Sam Esmail", "A TV show blogger...")]
        [TestCase(1, "Sam Sepiol", "A tech blogger...")]
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
        public async Task Get_all_bloggers()
        {
            var response = await _client.GetAsync("/bloggers");

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var bloggers = JsonConvert.DeserializeObject<List<BloggerOut>>(await response.Content.ReadAsStringAsync());

            bloggers.Count.ShouldBe(2);
        }

        [Test]
        public async Task Update_a_blogger_data()
        {
            await Login("elliot@blog.com", "Test@123");

            var responseBefore = await _client.GetAsync($"/bloggers/1");
            responseBefore.StatusCode.ShouldBe(HttpStatusCode.OK);
            var bloggerBefore = JsonConvert.DeserializeObject<BloggerOut>(await responseBefore.Content.ReadAsStringAsync());
            bloggerBefore.Id.ShouldBe(1);
            bloggerBefore.Name.ShouldBe("Sam Sepiol");
            bloggerBefore.Resume.ShouldBe("A tech blogger...");


            var bloggerUpdateIn = new BloggerUpdateIn
            {
                Name = "Zaqueu C.",
                Resume = "A .Net Core Blogger..."
            };
            var response = await _client.PutAsync("/bloggers", bloggerUpdateIn.ToStringContent());
            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);


            var responseAfter = await _client.GetAsync($"/bloggers/1");
            responseAfter.StatusCode.ShouldBe(HttpStatusCode.OK);
            var bloggerAfter = JsonConvert.DeserializeObject<BloggerOut>(await responseAfter.Content.ReadAsStringAsync());
            bloggerAfter.Id.ShouldBe(1);
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

            stats.PublishedPosts.ShouldBe(2);
            stats.DraftPosts.ShouldBe(0);
            stats.LatestComments.Count.ShouldBe(5);
        }

        #endregion
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        #region Categories

        [Test]
        [TestCase("Linux", "The Linux category description...")]
        [TestCase("Mr. Robot", "The Mr. Robot category description...")]
        public async Task Get_a_category(string name, string description)
        {
            var response = await _client.GetAsync($"/categories/{name}");

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
            var response = await _client.GetAsync("/categories");

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var categories = JsonConvert.DeserializeObject<List<CategoryOut>>(await response.Content.ReadAsStringAsync());

            categories.Count.ShouldBe(2);
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
                Category = "Linux",
                Tags = new List<string>{ "Tech" }
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
                Title = "A nex blog post",
                Resume = "A resume of the new blog post...",
                Body = "The body of the new blog post...",
                Category = "Linux",
                Authors = new List<int>{},
                Tags = new List<string>{ "Tech" }
            };

            var response = await _client.PostAsync("/posts", postIn.ToStringContent());
            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            var post = JsonConvert.DeserializeObject<PostOut>(await response.Content.ReadAsStringAsync());

            post.Title.ShouldBe(postIn.Title);
            post.Resume.ShouldBe(postIn.Resume);
            post.Body.ShouldBe(postIn.Body);
            post.Authors.Count.ShouldBe(1);
            post.Authors.ShouldContain(a => a == "Sam Sepiol");
            post.Tags.ShouldContain(t => t == "Tech");
        }





        #endregion
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    }
}
