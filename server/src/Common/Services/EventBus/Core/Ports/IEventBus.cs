namespace Common.Services.EventBus.Core.Ports;

public interface IEventBus
{
    Task PublishAsync<TEvent>(TEvent eventData) where TEvent : class;

    void Subscribe<TEvent>(Func<TEvent, Task> handler) where TEvent : class;

    void Unsubscribe<TEvent>(Func<TEvent, Task> handler) where TEvent : class;
}