namespace Infrastructure.Data.Repository;

using System.Linq.Expressions;
using Common.Data.Repository;
using Common.Entities;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

public class BaseRepository<E> : IBaseRepository<E>
    where E : BaseEntity
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<E> _dbSet;

    public BaseRepository(AppDbContext context)
    {
        this._context = context;
        this._dbSet = this._context.Set<E>();
    }

    public async Task<E?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await this._dbSet.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<E>> FindAllAsync(CancellationToken cancellationToken = default)
    {
        return await this._dbSet.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<E>> FindAsync(Expression<Func<E, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await this._dbSet.Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task<E?> FirstOrDefaultAsync(Expression<Func<E, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await this._dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task CreateAsync(E entity, CancellationToken cancellationToken = default)
    {
        await this._dbSet.AddAsync(entity, cancellationToken);
    }

    public async Task UpdateAsync(Guid id, Action<E> updateAction, CancellationToken cancellationToken = default)
    {
        var entity = await FindByIdAsync(id, cancellationToken);
        if (entity == null)
            throw new InvalidOperationException($"Entity {typeof(E).Name} with id {id} not found.");

        updateAction(entity);
        this._context.Entry(entity).State = EntityState.Modified;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await FindByIdAsync(id, cancellationToken);
        if (entity == null)
            throw new InvalidOperationException($"Entity {typeof(E).Name} with id {id} not found.");

        this._dbSet.Remove(entity);
    }

    public async Task<int> CountAsync(Expression<Func<E, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        if (predicate == null)
            return await this._dbSet.CountAsync(cancellationToken);

        return await this._dbSet.CountAsync(predicate, cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<E, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await this._dbSet.AnyAsync(predicate, cancellationToken);
    }
}
