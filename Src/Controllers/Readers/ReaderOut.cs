using Blog.Domain;

namespace Blog.Controllers.Readers
{
    public class ReaderOut
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ReaderOut(Reader reader)
        {
            Id = reader.Id;
            Name = reader.Name;
        }
    }
}
