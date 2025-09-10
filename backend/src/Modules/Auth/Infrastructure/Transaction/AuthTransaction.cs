using Auth.Core.Ports.Transaction;

namespace Auth.Infrastructure.Transaction;

using Auth.Infrastructure.Context;
using Common.Data.Transaction;

public class AuthTransaction : EfTransaction<AuthDbContext>, IAuthTransaction
{
    public AuthTransaction(AuthDbContext dbContext) : base(dbContext) {}
}