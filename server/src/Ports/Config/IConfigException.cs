namespace Ports.Config;

public interface IConfigException
{
    int Code { get; }
    string Message { get; }
}