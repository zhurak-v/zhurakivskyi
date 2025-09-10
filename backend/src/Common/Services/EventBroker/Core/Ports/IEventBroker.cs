namespace Common.Services.EventBroker.Core.Ports;

using Common.Services.EventBroker.Core.Entities;

public interface IEventBroker
{
    Task<bool> PublishAsync(EventEntity eventEntity);
    
    Task SubscribeAsync(string topic, CancellationToken ct);

}