using Blog.Domain;
using Blog.Identity;

namespace Blog.Database
{
    public static class DbSeeder
    {
        public static void Seed(this BlogContext db)
        {
            #region Users

            var bloggerUser = new User
            {
                UserName = "Sam",
                Email = "sam@blog.com"
            };

            var readerUser = new User
            {
                UserName = "Elliot",
                Email = "elliot@blog.com"
            };

            var otherReaderUser = new User
            {
                UserName = "Darlene",
                Email = "darlene@blog.com"
            };

            db.Users.AddRange(bloggerUser, readerUser, otherReaderUser);
            db.SaveChanges();

            #endregion
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            #region Bloggers, Tags and Posts

            var blogger = new Blogger
            {
                Name = "Sam",
                Resume = "A tech blogger...",
                CreatedAt = DateTime.Now,
                UserId = bloggerUser.Id
            };

            var techTag = new Tag
            {
                Name = "Tech",
                CreatedAt = DateTime.Now
            };

            var csharpTag = new Tag
            {
                Name = "C#",
                CreatedAt = DateTime.Now
            };

            var efCoreTag = new Tag
            {
                Name = "EF Core",
                CreatedAt = DateTime.Now
            };

            var post = new Post
            {
                Title = "A simple blog post title",
                Resume = "A very short blog post resume.",
                Body = "A blog post with many informations...",
                CreatedAt = DateTime.Now,
                Authors = new List<Blogger>{ blogger },
                Tags = new List<Tag>{ techTag, csharpTag, efCoreTag }
            };

            var otherPost = new Post
            {
                Title = "A other simple blog post title",
                Resume = "A other very short blog post resume.",
                Body = "A other blog post with many informations...",
                CreatedAt = DateTime.Now,
                Authors = new List<Blogger>{ blogger },
                Tags = new List<Tag>{ techTag }
            };

            db.Posts.AddRange(post, otherPost);
            db.SaveChanges();

            #endregion
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            #region Readers

            var reader = new Reader
            {
                Name = "Elliot",
                CreatedAt = DateTime.Now,
                UserId = readerUser.Id
            };

            var otherReader = new Reader
            {
                Name = "Darlene",
                CreatedAt = DateTime.Now,
                UserId = otherReaderUser.Id
            };

            db.Readers.AddRange(reader, otherReader);
            db.SaveChanges();

            #endregion
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            #region Comments

            var comment = new Comment
            {
                PostId = post.Id,
                PostRating = 5,
                Body = "A comment...",
                CreatedAt = DateTime.Now,
                ReaderId = reader.Id
            };

            var otherComment = new Comment
            {
                PostId = post.Id,
                PostRating = 1,
                Body = "A other comment...",
                CreatedAt = DateTime.Now,
                ReaderId = otherReader.Id
            };

            db.Comments.AddRange(comment, otherComment);
            db.SaveChanges();

            #endregion
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            #region Replies

            var reply = new Reply
            {
                CommentId = comment.Id,
                Body = "A comment reply...",
                CreatedAt = DateTime.Now,
                BloggerId = blogger.Id
            };

            var otherReply = new Reply
            {
                CommentId = comment.Id,
                Body = "A other comment reply...",
                CreatedAt = DateTime.Now,
                BloggerId = otherReader.Id
            };

            db.Replies.AddRange(reply, otherReply);
            db.SaveChanges();

            #endregion
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            #region Likes

            var like = new Like
            {
                CommentId = comment.Id,
                CreatedAt = DateTime.Now,
                ReaderId = reader.Id
            };

            var otherLike = new Like
            {
                CommentId = comment.Id,
                CreatedAt = DateTime.Now,
                ReaderId = otherReader.Id
            };

            db.Likes.AddRange(like, otherLike);
            db.SaveChanges();

            #endregion
        }
    }
}
