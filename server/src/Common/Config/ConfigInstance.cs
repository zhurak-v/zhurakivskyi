namespace Common.Config;

public static class ConfigInstance
{
    private static readonly ConfigService _instance = ConfigService.Load($"config/.{CurrentEnvironment}.env");

    public static T GetOrThrow<T>(string key)
    {
        return _instance.GetOrThrow<T>(key);
    }

    public static string CurrentEnvironment
    {
        get
        {
            return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!;
        }
    }

    public static bool IsProduction
    {
        get
        {
            return CurrentEnvironment == "Production";
        }
    }
}