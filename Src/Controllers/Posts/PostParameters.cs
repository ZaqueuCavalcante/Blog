namespace Blog.Controllers.Posts
{
    public class PostParameters : RequestParameters
    {
        public string? Tag { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }

        public bool MinDateIsGreaterThanMaxDate()
        {
            return MinDate != null && MaxDate != null && MinDate > MaxDate;
        }
    }
}
