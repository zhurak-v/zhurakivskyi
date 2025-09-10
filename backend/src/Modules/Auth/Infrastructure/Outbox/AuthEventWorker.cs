namespace Auth.Infrastructure.Outbox;

using Auth.Core.Ports.Outbox;
using Auth.Core.Ports.Repository;
using Common.Services.EventBroker.Core.Ports;
using Common.Services.EventBroker.Infrastructure;

public class AuthEventWorker : EventWorker, IAuthEventWorker
{
    public AuthEventWorker(IAuthOutboxRepository outboxRepository, IEventBroker broker)
        : base(outboxRepository, broker)
    {}
}