namespace User.Core.Ports.Repository;

using Common.Data.Repository;
using User.Core.Entities;
using User.Core.Enums;

public interface IUserRepository : IBaseRepository<UserEntity>
{
    public Task<UserEntity?> FindByEmail(string email, CancellationToken cancellationToken = default);
    public Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    public Task<IEnumerable<UserEntity>> FindUsersByRoleAsync(UserRole role, CancellationToken cancellationToken = default);
}