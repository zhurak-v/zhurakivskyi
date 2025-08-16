namespace Infrastructure.Data.Repository;

using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Profile.Core.Entities;
using Profile.Core.Ports.Repository;

public class ProfileRepository : BaseRepository<ProfileEntity>, IProfileRepository
{
    public ProfileRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<ProfileEntity>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return Enumerable.Empty<ProfileEntity>();

        searchTerm = searchTerm.ToLower();

        return await _dbSet
            .Where(p => (p.FirstName != null && p.FirstName.ToLower().Contains(searchTerm)) ||
                        (p.LastName != null && p.LastName.ToLower().Contains(searchTerm)))
            .ToListAsync(cancellationToken);
    }

    public async Task<ProfileEntity?> SearchByNickNameAsync(string nickName, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(nickName))
            return null;

        return await _dbSet.FirstOrDefaultAsync(p => p.NickName != null && p.NickName.ToLower() == nickName.ToLower(), cancellationToken);
    }


}