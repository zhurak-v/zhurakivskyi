namespace User.Infrastructure.Repository;

using Common.Services.EventBroker.Infrastructure;
using User.Infrastructure.Context;
using User.Core.Ports.Repository;

public class UserOutboxRepository : EventRepository<UserDbContext>, IUserOutboxRepository
{
    public UserOutboxRepository(UserDbContext context) : base(context) {}
}