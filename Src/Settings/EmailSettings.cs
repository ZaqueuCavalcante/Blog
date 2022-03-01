namespace Blog.Settings;

public class EmailSettings
{
    public string From { get; set; }
    public string SmtpServer { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    public EmailSettings(IConfiguration configuration) 
    {
        configuration.GetSection("Email").Bind(this);
    }
}
