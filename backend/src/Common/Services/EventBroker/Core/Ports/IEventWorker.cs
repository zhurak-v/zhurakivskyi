namespace Common.Services.EventBroker.Core.Ports;

public interface IEventWorker
{
    Task ProcessEventAsync(CancellationToken ct);
}