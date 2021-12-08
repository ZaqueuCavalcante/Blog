using Blog.Domain;

namespace Blog.Controllers.Bloggers
{
    public class BloggerOut
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Resume { get; set; }

        public BloggerOut(Blogger blogger)
        {
            Id = blogger.Id;
            Name = blogger.Name;
            Resume = blogger.Resume;
        }
    }
}
