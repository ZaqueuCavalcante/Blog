using Blog.Controllers.Posts;
using Blog.Domain;

namespace Blog.Services.Posts;

public interface IPostsService
{
    public Task<Post> CreatePost(int userId, PostIn dto);
}
