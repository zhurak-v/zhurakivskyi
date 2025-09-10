namespace Profile.Infrastructure.Repository;

using Common.Services.EventBroker.Infrastructure;
using Profile.Infrastructure.Context;
using Profile.Core.Ports.Repository;

public class ProfileOutboxRepository : EventRepository<ProfileDbContext>, IProfileOutboxRepository
{
    public ProfileOutboxRepository(ProfileDbContext context) : base(context) {}
}