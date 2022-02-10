namespace Blog.Settings;

public class DatabaseSettings
{
    public string ConnectionString { get; set; }

    public DatabaseSettings(IConfiguration configuration) 
    {
        ConnectionString = configuration["Database:ConnectionString"];
    }
}
