namespace User.Infrastructure.Repository;

using Common.Data.Repository;
using Microsoft.EntityFrameworkCore;
using User.Core.Entities;
using User.Core.Enums;
using User.Core.Ports.Repository;
using User.Infrastructure.Context;

public class UserRepository : BaseRepository<UserEntity, UserDbContext>, IUserRepository
{
    public UserRepository(UserDbContext context) : base(context) { }

    public async Task<UserEntity?> FindByEmail(string email, CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await DbSet.AnyAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<IEnumerable<UserEntity>> FindUsersByRoleAsync(UserRole role, CancellationToken cancellationToken = default)
    {
        return await DbSet.Where(u => u.Role == role).ToListAsync(cancellationToken);
    }
}