namespace Profile.Core.Ports.Repository;

using Common.Data.Repository;
using Profile.Core.Entities;

public interface IProfileRepository : IBaseRepository<ProfileEntity>
{
    Task<IEnumerable<ProfileEntity>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<ProfileEntity?> SearchByNickNameAsync(string nickName, CancellationToken cancellationToken = default);
}