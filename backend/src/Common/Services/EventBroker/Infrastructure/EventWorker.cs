namespace Common.Services.EventBroker.Infrastructure;

using Common.Services.EventBroker.Core.Ports;
using Common.Services.EventBroker.Core.Entities;

public class EventWorker : IEventWorker
{
    private readonly IEventRepository _eventRepository;
    private readonly IEventBroker _eventBroker;

    public EventWorker(IEventRepository eventRepository, IEventBroker eventBroker)
    {
        _eventRepository = eventRepository;
        _eventBroker = eventBroker;
    }
    
    public async Task ProcessEventAsync(CancellationToken ct)
    {
        var events = await _eventRepository.GetByStatusAsync(EventStatus.New);

        foreach (var evt in events)
        {
            try
            {
                await _eventBroker.PublishAsync(evt);
                await _eventRepository.MarkProcessedAsync(evt);
            }
            catch
            {
                await _eventRepository.MarkFailedAsync(evt);
            }
        }
    }
}
