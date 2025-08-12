namespace Ports.Repository;

using Core.Entity;

public interface IProfileRepository : IBaseRepository<ProfileEntity>
{
    Task<ProfileEntity?> FindByNickNameAsync(string nickName, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProfileEntity>> FindByFullNameAsync(string partialName, CancellationToken cancellationToken = default);
}