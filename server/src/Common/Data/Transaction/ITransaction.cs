namespace Common.Data.Transaction;

public interface ITransaction : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task ExecuteInTransactionAsync(Func<Task> operation);
}
