namespace Common.Config;

using Common.Config.Interface;

internal class ConfigException : Exception, IConfigException
{
    private readonly int _code;
    private readonly string _message;

    public ConfigException(int code, string message)
        : base(message)
    {
        this._code = code;
        this._message = message;
    }

    public ConfigException(int code, string message, Exception innerException)
        : base(message, innerException)
    {
        this._code = code;
        this._message = message;
    }

    public int GetCode()
    {
        return this._code;
    }

    public string GetMessage()
    {
        return this._message;
    }

    public override string ToString()
    {
        return $"ConfigException: Code={this._code}, Message={this._message}";
    }
}