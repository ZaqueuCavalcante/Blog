using Blog.Exceptions;

namespace Blog.Domain
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<Post> Posts { get; set; }

        public Category(
            string name,
            string description
        ) {
            SetName(name);
            SetDescription(description);
            CreatedAt = DateTime.Now;
        }

        private void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Category's name cannot be empty.");

            if (name.Count() < 3)
                throw new DomainException("Category's name cannot be that short.");

            Name = name;
        }

        private void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new DomainException("Category's description cannot be empty.");

            if (description.Count() < 10)
                throw new DomainException("Category's description cannot be that short.");

            Description = description;
        }
    }
}
