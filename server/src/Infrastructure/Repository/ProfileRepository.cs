namespace Infrastructure.Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Entity;
using Infrastructure.Repository.Configuration;
using Microsoft.EntityFrameworkCore;
using Ports.Repository;

public class ProfileRepository : BaseRepository<ProfileEntity>, IProfileRepository
{
    public ProfileRepository(RepositoryDbContext context) : base(context) { }

    public async Task<ProfileEntity?> FindByNickNameAsync(string nickName, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(nickName))
            return null;

        return await this._dbSet.FirstOrDefaultAsync(p => p.NickName == nickName, cancellationToken);
    }

    public async Task<IEnumerable<ProfileEntity>> FindByFullNameAsync(string partialName, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(partialName))
            return new List<ProfileEntity>();

        return await this._dbSet
            .Where(p => p.FullName != null && EF.Functions.Like(p.FullName, $"%{partialName}%"))
            .ToListAsync(cancellationToken);
    }
}