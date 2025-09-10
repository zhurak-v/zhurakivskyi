namespace Common.Services.EventBroker.Core.Ports;

public interface IOutboxListener
{
    Task StartAsync(CancellationToken ct);
}