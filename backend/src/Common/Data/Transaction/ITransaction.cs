namespace Common.Data.Transaction;

public interface ITransaction
{
    Task ExecuteAsync(Func<Task> action);
}