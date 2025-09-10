namespace Profile.Core.Ports.Repository;

using Common.Data.Repository;
using Profile.Core.Entities;

public interface IProfileRepository : IBaseRepository<ProfileEntity>
{
    Task<ProfileEntity?> FindByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default);
}