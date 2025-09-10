namespace Auth.Infrastructure.Repository;

using Common.Data.Repository;
using Auth.Core.Entities;
using Auth.Core.Ports.Repository;
using Auth.Infrastructure.Context;

public class AuthRepository : BaseRepository<AuthRegisterEntity, AuthDbContext>, IAuthRegisterRepository
{
    public AuthRepository(AuthDbContext context) : base(context) {}
}