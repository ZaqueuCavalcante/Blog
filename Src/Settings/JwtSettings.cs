namespace Blog.Settings;

public class JwtSettings
{
    public string SecurityKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpirationTimeInMinutes { get; set; }
    public int RefreshTokenExpirationTimeInMinutes { get; set; }

    public JwtSettings(IConfiguration configuration) 
    {
        SecurityKey = configuration["JWT:SecurityKey"];
        Issuer = configuration["JWT:Issuer"];
        Audience = configuration["JWT:Audience"];
        ExpirationTimeInMinutes = int.Parse(configuration["JWT:ExpirationTimeInMinutes"]);
        RefreshTokenExpirationTimeInMinutes = int.Parse(configuration["JWT:RefreshTokenExpirationTimeInMinutes"]);
    }
}
