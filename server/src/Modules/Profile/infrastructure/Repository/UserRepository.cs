namespace Profile.Infrastructure.Repository;

using Common.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Profile.Core.Entities;
using Profile.Core.Ports.Repository;
using Profile.Infrastructure.Repository.Context;

public class ProfileRepository : BaseRepository<ProfileEntity, ProfileDbContext>, IProfileRepository
{
    public ProfileRepository(ProfileDbContext context) : base(context) { }

    public async Task<IEnumerable<ProfileEntity>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return Array.Empty<ProfileEntity>();

        searchTerm = searchTerm.ToLower();

        return await this._dbSet
            .Where(p => (p.FirstName != null && EF.Functions.Like(p.FirstName, $"%{searchTerm}%")) ||
                        (p.LastName != null && EF.Functions.Like(p.LastName, $"%{searchTerm}%")))
            .ToListAsync(cancellationToken);
    }

    public async Task<ProfileEntity?> SearchByNickNameAsync(string nickName, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(nickName))
            return null;

        return await this._dbSet.FirstOrDefaultAsync(
            p => p.NickName != null && 
                 string.Equals(p.NickName, nickName, StringComparison.OrdinalIgnoreCase),
            cancellationToken);
    }


}