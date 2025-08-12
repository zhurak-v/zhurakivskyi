namespace Ports.Config;

public interface IConfigService
{
    T GetOrThrow<T>(string key);
}