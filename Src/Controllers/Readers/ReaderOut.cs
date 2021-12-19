using Blog.Domain;

namespace Blog.Controllers.Readers
{
    public class ReaderOut
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static ReaderOut New(Reader reader)
        {
            return new ReaderOut
            {
                Id = reader.Id,
                Name = reader.Name
            };
        }
    }
}
