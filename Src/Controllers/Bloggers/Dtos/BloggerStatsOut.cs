using Blog.Domain;
using Blog.Extensions;

namespace Blog.Controllers.Bloggers;

public class BloggerStatsOut
{
    public int PublishedPosts { get; set; }
    public int DraftPosts { get; set; }
    public List<object> LatestComments { get; set; }

    public static BloggerStatsOut New(int publishedPosts, int draftPosts, List<Comment> latestComments)
    {
        return new BloggerStatsOut
        {
            PublishedPosts = publishedPosts,
            DraftPosts = draftPosts,
            LatestComments = latestComments?.Select(c => (object) new { Date = c.CreatedAt.Format(), Body = c.Body }).ToList()
        };
    }
}
