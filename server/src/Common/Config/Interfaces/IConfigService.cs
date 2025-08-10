namespace Common.Config.Interface;

public interface IConfigService
{
    T GetOrThrow<T>(string key);
}