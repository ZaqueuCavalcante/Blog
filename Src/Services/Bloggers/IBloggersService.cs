using Blog.Domain;

namespace Blog.Services.Bloggers;

public interface IBloggersService
{
    public Task<Blogger> CreateBlogger(string name, string resume, string email, string password);

    public Task<Blogger> GetBlogger(int id);

    public Task<List<Blogger>> GetBloggers();

    public Task UpdateBlogger(int userId, string name, string resume);
}
