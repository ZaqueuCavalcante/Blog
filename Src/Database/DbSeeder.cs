using Blog.Domain;
using Blog.Identity;

namespace Blog.Database
{
    public static class DbSeeder
    {
        public static void Seed(this BlogContext db)
        {
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

            db.Users.AddRange(bloggerUser, readerUser);
            db.SaveChanges();

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

            var post = new Post
            {
                Title = "A simple blog post title",
                Resume = "A very short blog post resume.",
                Body = "A blog post with many informations...",
                CreatedAt = DateTime.Now,
                Authors = new List<Blogger>{ blogger },
                Tags = new List<Tag>{ techTag, csharpTag }
            };

            db.Posts.Add(post);
            db.SaveChanges();

            var reader = new Reader
            {
                Name = "Elliot",
                CreatedAt = DateTime.Now,
                UserId = readerUser.Id
            };

            db.Readers.Add(reader);
            db.SaveChanges();

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
                PostRating = 2,
                Body = "A other comment...",
                CreatedAt = DateTime.Now,
                ReaderId = reader.Id
            };

            db.Comments.AddRange(comment, otherComment);
            db.SaveChanges();

            var reply = new Reply
            {
                CommentId = comment.Id,
                Body = "A comment reply...",
                CreatedAt = DateTime.Now,
                BloggerId = blogger.Id
            };
            db.Replies.Add(reply);

            db.SaveChanges();

            var like = new Like
            {
                CommentId = comment.Id,
                CreatedAt = DateTime.Now,
                ReaderId = reader.Id
            };
            db.Likes.Add(like);

            db.SaveChanges();

            Console.WriteLine("Database seed completed...");
        }
    }
}
