namespace Blog.Domain
{
    public class Network
    {
        public int Id { get; }

        public int UserId { get; set; }

        public string Name { get; set; }

        public string Uri { get; set; }





        public static readonly HashSet<string> Alloweds = new HashSet<string>
        {
            { "GitHub" }, { "Twitter" }, { "YouTube" }, { "Instagram" }, { "LinkedIn" }
        };
    }
}
