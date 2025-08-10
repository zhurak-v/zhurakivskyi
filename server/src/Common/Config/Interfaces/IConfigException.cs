namespace Common.Config.Interface;

public interface IConfigException
{
    public int GetCode();
    public string GetMessage();
}