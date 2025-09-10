namespace Profile.Infrastructure.Outbox;

using Profile.Core.Ports.Outbox;
using Common.Services.EventBroker.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

public class ProfileOutboxEventListener : OutboxListener<IProfileEventWorker>, IProfileOutboxEventListener
{
    public ProfileOutboxEventListener(
        IServiceScopeFactory scopeFactory,
        string connectionString,
        string channelName = "profile_outbox_channel"
    ) : base(scopeFactory, connectionString, channelName)
    {}
}