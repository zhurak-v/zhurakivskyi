namespace Ports.Repository;

using Core.Entity;

public interface IUserRepository : IBaseRepository<UserEntity>
{
    public Task<UserEntity?> FindByEmail(string email, CancellationToken cancellationToken = default);
    public Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    public Task<IEnumerable<UserEntity>> FindUsersByRoleAsync(UserRole role, CancellationToken cancellationToken = default);
}