namespace Common.Data.Repository;

using System.Linq.Expressions;
using Common.Entities;

public interface IBaseRepository<E> where E : BaseEntity
{
    Task<E?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<E>> FindAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<E>> FindAsync(Expression<Func<E, bool>> predicate, CancellationToken cancellationToken = default);
    Task<E?> FirstOrDefaultAsync(Expression<Func<E, bool>> predicate, CancellationToken cancellationToken = default);
    Task CreateAsync(E entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid id, Action<E> updateAction, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<int> CountAsync(Expression<Func<E, bool>>? predicate = null, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Expression<Func<E, bool>> predicate, CancellationToken cancellationToken = default);
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}