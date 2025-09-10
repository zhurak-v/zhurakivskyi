namespace Auth.Infrastructure.Repository;

using Common.Services.EventBroker.Infrastructure;
using Auth.Infrastructure.Context;
using Auth.Core.Ports.Repository;


public class AuthOutboxRepository : EventRepository<AuthDbContext>, IAuthOutboxRepository
{
    public AuthOutboxRepository(AuthDbContext context) : base(context) {}
}