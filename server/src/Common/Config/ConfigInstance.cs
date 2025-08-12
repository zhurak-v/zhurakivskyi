namespace Common.Config;

using Ports.Config;

public class ConfigInstance : IConfigInstance
{
    private readonly ConfigService _instance;

    public ConfigInstance()
    {
        this._instance = ConfigService.Load($"config/.{CurrentEnvironment}.env");
    }

    public T GetOrThrow<T>(string key)
    {
        return this._instance.GetOrThrow<T>(key);
    }

    public string CurrentEnvironment
    {
        get
        {
            return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!;
        }
    }

    public bool IsProduction
    {
        get
        {
            return CurrentEnvironment == "Production";
        }
    }
}
