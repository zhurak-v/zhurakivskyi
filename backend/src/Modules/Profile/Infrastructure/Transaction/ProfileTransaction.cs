using Profile.Core.Ports.Transaction;

namespace Profile.Infrastructure.Transaction;

using Profile.Infrastructure.Context;
using Common.Data.Transaction;

public class ProfileTransaction : EfTransaction<ProfileDbContext>, IProfileTransaction
{
    public ProfileTransaction(ProfileDbContext dbContext) : base(dbContext) {}
}