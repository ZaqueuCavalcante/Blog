namespace Blog.Controllers.Users
{
    public class RefreshTokenOut
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public List<string>? Errors { get; set; }
    }
}
