using Blog.Exceptions;

namespace Blog.Domain
{
    public class Blogger
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; }

        public string Resume { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<Post> Posts { get; set; }

        public Blogger(
            string name,
            string resume,
            int userId
        ) {
            SetName(name);
            SetResume(resume);
            UserId = userId;
            CreatedAt = DateTime.Now;
        }

        private void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Blogger's name cannot be empty.");

            if (name.Count() < 3)
                throw new DomainException("Blogger's name cannot be that short.");

            Name = name;
        }

        private void SetResume(string resume)
        {
            if (string.IsNullOrWhiteSpace(resume))
                throw new DomainException("Blogger's resume cannot be empty.");

            if (resume.Count() < 10)
                throw new DomainException("Blogger's resume cannot be that short.");

            Resume = resume;
        }

        public void Update(
            string name,
            string resume
        ) {
            SetName(name);
            SetResume(resume);    
        }
    }
}
