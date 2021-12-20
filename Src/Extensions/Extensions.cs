namespace Blog.Extensions
{
    public static class Extensions
    {
        public static string Format(this DateTime date)
        {
            return date.ToString("dd/MM/yyyy HH:mm");
        }

        public static string GetRoot(this HttpRequest request)
        {
            return request.Scheme + "://" + request.Host.Value + "/";
        }
    }
}
