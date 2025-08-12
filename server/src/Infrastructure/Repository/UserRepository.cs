namespace Infrastructure.Repository;

using Core.Entity;
using Infrastructure.Repository.Configuration;
using Microsoft.EntityFrameworkCore;
using Ports.Repository;

public class UserRepository : BaseRepository<UserEntity>, IUserRepository
{
    public UserRepository(RepositoryDbContext context) : base(context) { }

    public async Task<UserEntity?> FindByEmail(string email, CancellationToken cancellationToken = default)
    {
        return await this._dbSet.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await this._dbSet.AnyAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<IEnumerable<UserEntity>> FindUsersByRoleAsync(UserRole role, CancellationToken cancellationToken = default)
    {
        return await this._dbSet.Where(u => u.Role == role).ToListAsync(cancellationToken);
    }
}