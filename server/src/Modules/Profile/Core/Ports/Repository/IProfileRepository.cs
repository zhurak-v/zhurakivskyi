namespace Profile.Core.Ports.Repository;

using Common.Data.Repository;
using Profile.Core.Entities;

public interface IProfileRepository : IBaseRepository<ProfileEntity>
{
    public Task<IEnumerable<ProfileEntity>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default);
    public Task<ProfileEntity?> SearchByNickNameAsync(string NickName, CancellationToken cancellationToken = default);
}