using Blog.Domain;

namespace Blog.Controllers.Tags
{
    public class TagOut
    {
        public string Name { get; set; }
        public string CreatedAt { get; set; }

        public List<string> Posts { get; set; }

        public TagOut(Tag tag)
        {
            Name = tag.Name;
            CreatedAt = tag.CreatedAt.ToString("dd/MM/yyyy HH:mm");
            Posts = tag.Posts?.Select(b => b.Title).ToList();
        }
    }
}