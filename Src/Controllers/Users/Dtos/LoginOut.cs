namespace Blog.Controllers.Users;

public class LoginOut
{
    public string AccessToken { get; set; }
    public string TokenType { get; set; }
    public string ExpiresInMinutes { get; set; }
    public string RefreshToken { get; set; }
    public string Scope { get; set; }        
}
