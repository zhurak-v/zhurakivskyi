namespace Common.Services.EventBroker.Infrastructure;

using Common.Services.EventBroker.Core.Ports;
using Microsoft.Extensions.Hosting;

public class KafkaConsumerService : BackgroundService
{
    private readonly IEventBroker _eventBroker;

    public KafkaConsumerService(IEventBroker eventBroker)
    {
        _eventBroker = eventBroker;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _eventBroker.SubscribeAsync("auth-events", stoppingToken);
        await _eventBroker.SubscribeAsync("user-events", stoppingToken);
        await _eventBroker.SubscribeAsync("profile-events", stoppingToken);
    }
}