namespace Infrastructure.Data.Repository;

using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using User.Core.Entities;
using User.Core.Enums;
using User.Core.Ports.Repository;

public class UserRepository : BaseRepository<UserEntity>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context) { }

    public async Task<UserEntity?> FindByEmail(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<IEnumerable<UserEntity>> FindUsersByRoleAsync(UserRole role, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(u => u.Role == role).ToListAsync(cancellationToken);
    }
}