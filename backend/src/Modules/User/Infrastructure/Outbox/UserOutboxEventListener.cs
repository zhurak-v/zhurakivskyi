namespace User.Infrastructure.Outbox;

using User.Core.Ports.Outbox;
using Common.Services.EventBroker.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

public class UserOutboxEventListener : OutboxListener<IUserEventWorker>, IUserOutboxEventListener
{
    public UserOutboxEventListener(
        IServiceScopeFactory scopeFactory,
        string connectionString,
        string channelName = "user_outbox_channel"
    ) : base(scopeFactory, connectionString, channelName)
    {}
}