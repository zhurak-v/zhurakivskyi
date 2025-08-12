namespace Infrastructure.Repository;

using System.Linq.Expressions;
using Core.Entity;
using Infrastructure.Repository.Configuration;
using Microsoft.EntityFrameworkCore;
using Ports.Repository;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly RepositoryDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(RepositoryDbContext context)
    {
        this._context = context;
        this._dbSet = this._context.Set<T>();
    }

    public async Task<T?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await this._dbSet.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<T>> FindAllAsync(CancellationToken cancellationToken = default)
    {
        return await this._dbSet.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await this._dbSet.Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await this._dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await this._dbSet.AddAsync(entity, cancellationToken);
    }

    public async Task UpdateAsync(Guid id, Action<T> updateAction, CancellationToken cancellationToken = default)
    {
        var entity = await FindByIdAsync(id, cancellationToken);
        if (entity == null)
            throw new InvalidOperationException($"Entity {typeof(T).Name} with id {id} not found.");

        updateAction(entity);
        this._context.Entry(entity).State = EntityState.Modified;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await FindByIdAsync(id, cancellationToken);
        if (entity == null)
            throw new InvalidOperationException($"Entity {typeof(T).Name} with id {id} not found.");

        this._dbSet.Remove(entity);
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        if (predicate == null)
            return await this._dbSet.CountAsync(cancellationToken);

        return await this._dbSet.CountAsync(predicate, cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await this._dbSet.AnyAsync(predicate, cancellationToken);
    }
}
