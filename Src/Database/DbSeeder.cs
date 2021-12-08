using Blog.Domain;

namespace Blog.Database
{
    public static class DbSeeder
    {
        public static void Seed(this BlogContext db)
        {
            var blogger = new Blogger
            {
                Name = "Sam",
                Resume = "A tech blogger...",
                CreatedAt = DateTime.Now
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
                CreatedAt = DateTime.Now
            };

            db.Readers.Add(reader);
            db.SaveChanges();

            var comment = new Comment
            {
                PostId = post.Id,
                Body = "A comment...",
                CreatedAt = DateTime.Now,
                ReaderId = reader.Id
            };

            db.Comments.Add(comment);
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

            Console.WriteLine("Database seed completed...");
        }
    }
}
