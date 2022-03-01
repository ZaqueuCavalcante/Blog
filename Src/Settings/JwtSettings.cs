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
        configuration.GetSection("JWT").Bind(this);
    }
}
