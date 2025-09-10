namespace Profile.Infrastructure.Repository;

using Common.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Profile.Core.Entities;
using Profile.Core.Ports.Repository;
using Profile.Infrastructure.Context;

public class ProfileRepository : BaseRepository<ProfileEntity, ProfileDbContext>, IProfileRepository
{
    public ProfileRepository(ProfileDbContext context) : base(context) { }

    public async Task<ProfileEntity?> FindByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(p => p.PhoneNumber == phoneNumber, cancellationToken);
    }
}