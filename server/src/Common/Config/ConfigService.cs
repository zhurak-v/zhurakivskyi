namespace Common.Config;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Common.Config.Interface;
using DotNetEnv;

internal class ConfigService : IConfigService
{
    private readonly string _envPath;
    private static ConfigService _instance = null!;

    [DoesNotReturn]
    private void ThrowVariableNotSet(string variableName)
    {
        throw new ConfigException(
            404,
            $"Environment variable '{variableName}' is not set"
        );
    }

    [DoesNotReturn]
    private void ThrowCastError(string variableName, Type targetType, string? value = null, Exception? inner = null)
    {
        var message = value is null
            ? $"Environment variable '{variableName}' cannot be cast to {targetType.Name}"
            : $"Cannot cast value '{value}' of '{variableName}' to {targetType.Name}";

        throw inner is null
            ? new ConfigException(422, message)
            : new ConfigException(422, message, inner);
    }

    [DoesNotReturn]
    private void ThrowUnsupportedType(string variableName, Type type)
    {
        throw new ConfigException(
            500,
            $"Unsupported type: {type.Name} for environment variable '{variableName}'"
        );
    }

    private ConfigService(string envPath)
    {
        this._envPath = envPath;
        if (File.Exists(envPath))
        {
            Env.Load(envPath);
        }
    }

    private T Cast<T>(string value, string key)
    {
        try
        {
            var targetType = typeof(T);

            if (targetType == typeof(bool))
            {
                var lowered = value.ToLowerInvariant();
                if (lowered is "true" or "1" or "yes" or "on")
                    return (T)(object)true;
                if (lowered is "false" or "0" or "no" or "off")
                    return (T)(object)false;

                ThrowCastError(key, targetType, value);
            }
            else
            {
                return (T)Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture)!;
            }

            ThrowUnsupportedType(key, targetType);
        }
        catch (Exception ex)
        {
            ThrowCastError(key, typeof(T), value, ex);
        }

        return default!;
    }

    public static ConfigService Load(string envPath)
    {
        if (_instance is null)
        {
            _instance = new ConfigService(envPath);
        }
        return _instance;
    }

    public T GetOrThrow<T>(string key)
    {
        var value = Environment.GetEnvironmentVariable(key);
        if (value is null)
        {
            ThrowVariableNotSet(key);
        }

        return this.Cast<T>(value, key);
    }

}