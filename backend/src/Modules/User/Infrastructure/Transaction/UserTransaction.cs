using User.Core.Ports.Transaction;

namespace User.Infrastructure.Transaction;

using User.Infrastructure.Context;
using Common.Data.Transaction;

public class UserTransaction : EfTransaction<UserDbContext>, IUserTransaction
{
    public UserTransaction(UserDbContext dbContext) : base(dbContext) {}
}