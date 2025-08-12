namespace Common.Config;

using Ports.Config;

internal class ConfigException : Exception, IConfigException
{
    public int Code { get; }

    public override string Message
    {
        get
        {
            return base.Message;
        }
    }

    public ConfigException(int code, string message)
        : base(message)
    {
        Code = code;
    }

    public ConfigException(int code, string message, Exception innerException)
        : base(message, innerException)
    {
        Code = code;
    }

    public override string ToString()
    {
        return $"ConfigException: Code={Code}, Message={Message}";
    }
}
