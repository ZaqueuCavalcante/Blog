using Blog.Exceptions;

namespace Blog.Domain
{
    public class Tag
    {
        public int Id { get; }

        public string Name { get; private set; }

        public DateTime CreatedAt { get; }

        public List<Post> Posts { get; }

        public Tag(string name)
        {
            SetName(name);
            CreatedAt = DateTime.Now;
        }

        private void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Tag's name cannot be empty.");

            if (name.Count() < 3)
                throw new DomainException("Tag's name cannot be that short.");

            Name = name;
        }
    }
}
