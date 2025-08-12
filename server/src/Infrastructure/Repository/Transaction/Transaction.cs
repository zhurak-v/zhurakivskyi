namespace Infrastructure.Repository.Transaction;

using Infrastructure.Repository.Configuration;
using Ports.Repository;

public class Transaction : ITransaction
{
    private readonly RepositoryDbContext _context;

    public Transaction(RepositoryDbContext context)
    {
        this._context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await this._context.SaveChangesAsync(cancellationToken);
    }

    public async Task ExecuteInTransactionAsync(Func<Task> operation)
    {
        await using var transaction = await this._context.Database.BeginTransactionAsync();

        try
        {
            await operation();

            await this._context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public void Dispose()
    {
        this._context.Dispose();
    }
}