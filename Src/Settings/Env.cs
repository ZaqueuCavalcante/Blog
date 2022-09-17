namespace Blog.Settings;

public static class Env
{
    private static string Testing = nameof(Testing);
    private static string Development = nameof(Development);
    private static string Staging = nameof(Staging);
    private static string Production = nameof(Production);

    public static void SetAsTesting()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", Testing);
    }

    public static bool IsTesting()
    {
        return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Testing;
    }

    public static bool IsDevelopment()
    {
        return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower() == Development.ToLower();
    }

    public static bool IsStaging()
    {
        return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower() == Staging.ToLower();
    }

    public static bool IsProduction()
    {
        return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower() == Production.ToLower();
    }

    public static bool IsTestingOrDevelopmentOrStaging()
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        return env == Testing || env?.ToLower() == Development.ToLower() || env?.ToLower() == Staging.ToLower();
    }
}
