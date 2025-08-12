namespace Ports.Config;

public interface IConfigInstance
{
    T GetOrThrow<T>(string key);

    string CurrentEnvironment { get; }

    bool IsProduction { get; }
}