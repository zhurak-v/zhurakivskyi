namespace Auth.Infrastructure.Outbox;

using Auth.Core.Ports.Outbox;
using Common.Services.EventBroker.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

public class AuthOutboxEventListener : OutboxListener<IAuthEventWorker>, IAuthOutboxEventListener
{
    public AuthOutboxEventListener(
        IServiceScopeFactory scopeFactory,
        string connectionString,
        string channelName = "auth_outbox_channel"
    ) : base(scopeFactory, connectionString, channelName)
    {
    }
}