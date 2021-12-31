namespace Blog.Controllers
{
    public abstract class RequestParameters
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = (value > maxPageSize) ? maxPageSize : value; }
        }
        private int _pageSize = 2;
        const int maxPageSize = 10;
    }
}
