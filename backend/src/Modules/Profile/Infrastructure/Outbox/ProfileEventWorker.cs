namespace Profile.Infrastructure.Outbox;

using Profile.Core.Ports.Outbox;
using Profile.Core.Ports.Repository;
using Common.Services.EventBroker.Core.Ports;
using Common.Services.EventBroker.Infrastructure;

public class ProfileEventWorker : EventWorker, IProfileEventWorker
{
    public ProfileEventWorker(IProfileOutboxRepository outboxRepository, IEventBroker broker)
        : base(outboxRepository, broker)
    {}
}