namespace Blog.Controllers.Posts
{
    public class PostParameters : RequestParameters
    {
        public int? CategoryId { get; set; }
        public int? TagId { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }

        public bool MinDateIsGreaterThanMaxDate()
        {
            return MinDate != null && MaxDate != null && MinDate > MaxDate;
        }
    }
}
