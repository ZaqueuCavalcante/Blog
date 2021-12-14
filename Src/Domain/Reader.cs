using Blog.Exceptions;

namespace Blog.Domain
{
    public class Reader
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public Reader(
            string name,
            int userId
        ) {
            SetName(name);
            UserId = userId;
            CreatedAt = DateTime.Now;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Readers's name cannot be empty.");

            if (name.Count() < 3)
                throw new DomainException("Readers's name cannot be that short.");

            Name = name;
        }
    }
}
