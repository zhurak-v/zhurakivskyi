namespace User.Infrastructure.Outbox;

using User.Core.Ports.Outbox;
using User.Core.Ports.Repository;
using Common.Services.EventBroker.Core.Ports;
using Common.Services.EventBroker.Infrastructure;

public class UserEventWorker : EventWorker, IUserEventWorker
{
    public UserEventWorker(IUserOutboxRepository outboxRepository, IEventBroker broker)
        : base(outboxRepository, broker)
    {}
}