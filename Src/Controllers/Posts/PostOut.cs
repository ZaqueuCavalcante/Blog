using Blog.Domain;
using Blog.Extensions;

namespace Blog.Controllers.Posts
{
    public class PostOut
    {
        public int Id { get; set; }
        public int? PinnedCommentId { get; set; }
        public string Title { get; set; }
        public string Resume { get; set; }
        public string Body { get; set; }
        public byte Rating { get; set; }
        public string CreatedAt { get; set; }
        public List<PostAuthorOut> Authors { get; set; }
        public List<CommentOut> Comments { get; set; }
        public PostCategoryOut Category { get; set; }
        public List<PostTagOut> Tags { get; set; }

        public static PostOut New(Post post, Category? category = null, string? url = null)
        {
            return new PostOut
            {
                Id = post.Id,
                PinnedCommentId = post.PinnedCommentId,
                Title = post.Title,
                Resume = post.Resume,
                Body = post.Body,
                Rating = post.GetRating(),
                CreatedAt = post.CreatedAt.Format(),
                Authors = post.Authors?.Select(b => new PostAuthorOut{ Name = b.Name, Link = url + "bloggers/" + b.Id }).ToList(),
                Comments = post.Comments?.Select(c => CommentOut.New(c)).ToList(),
                Category = new PostCategoryOut{ Name = category?.Name, Link = url + "categories/" + category?.Id },
                Tags = post.Tags?.Select(t => new PostTagOut{ Name = t.Name, Link = url + "tags/" + t.Id }).ToList()
            };
        }

        public static PostOut NewWithoutComments(Post post, Category? category = null, string? url = null)
        {
            return new PostOut
            {
                Id = post.Id,
                PinnedCommentId = post.PinnedCommentId,
                Title = post.Title,
                Resume = post.Resume,
                Body = post.Body,
                Rating = post.GetRating(),
                CreatedAt = post.CreatedAt.Format(),
                Authors = post.Authors?.Select(b => new PostAuthorOut{ Name = b.Name, Link = url + "bloggers/" + b.Id }).ToList(),
                Comments = new List<CommentOut>(),
                Category = new PostCategoryOut{ Name = category?.Name, Link = url + "categories/" + category?.Id },
                Tags = post.Tags?.Select(t => new PostTagOut{ Name = t.Name, Link = url + "tags/" + t.Id }).ToList()
            };
        }
    }

    public class PostAuthorOut
    {
        public string Name { get; set; }
        public string Link { get; set; }
    }

    public class PostCategoryOut
    {
        public string Name { get; set; }
        public string Link { get; set; }
    }

    public class PostTagOut
    {
        public string Name { get; set; }
        public string Link { get; set; }
    }
}
