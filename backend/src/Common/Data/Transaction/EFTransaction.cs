namespace Common.Data.Transaction;

using Microsoft.EntityFrameworkCore;

public class EfTransaction<TContext> : ITransaction
    where TContext : DbContext
{
    private readonly TContext _dbContext;

    public EfTransaction(TContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ExecuteAsync(Func<Task> action)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            await action();
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}