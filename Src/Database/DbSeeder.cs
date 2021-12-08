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
                Resume = "A tech blogger..."
            };

            var reader = new Reader
            {
                Name = "Elliot"
            };

            var comment = new Comment
            {
                Body = "A comment...",
                CreatedAt = DateTime.Now,
                Reader = reader
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
                Comments = new List<Comment>{ comment },
                Tags = new List<Tag>{ techTag, csharpTag }
            };

            db.Posts.Add(post);
            db.SaveChanges();

            var commentId = db.Comments.First(x => x.Body == "A comment...").Id;

            var reply = new Comment
            {
                Body = "A comment reply...",
                CreatedAt = DateTime.Now,
                Blogger = blogger,
                RepliedCommentId = commentId
            };
            db.Comments.Add(reply);

            db.SaveChanges();

            Console.WriteLine("Database seed completed...");
        }
    }
}
