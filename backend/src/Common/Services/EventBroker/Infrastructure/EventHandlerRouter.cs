namespace Common.Services.EventBroker.Infrastructure;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Services.EventBroker.Core.Entities;
using Common.Services.EventBroker.Core.Ports;
using Microsoft.Extensions.DependencyInjection;

public class EventHandlerRouter : IEventHandlerRouter
{
    private readonly ConcurrentDictionary<string, List<Type>> _handlers = new();
    private readonly IServiceScopeFactory _scopeFactory;

    public EventHandlerRouter(IServiceScopeFactory scopeFactory, IServiceProvider serviceProvider)
    {
        _scopeFactory = scopeFactory;

        using var scope = serviceProvider.CreateScope();
        var handlers = scope.ServiceProvider.GetServices<IEventHandler>();

        foreach (var handler in handlers)
        {
            var handlerType = handler.GetType();

            if (_handlers.TryGetValue(handler.EventName, out var existing))
            {
                if (!existing.Contains(handlerType))
                    existing.Add(handlerType);

                continue;
            }

            _handlers[handler.EventName] = [ handlerType ];
            Console.WriteLine($"[Router] Registered handler: {handler.EventName} → {handlerType.Name}");
        }
    }
    
    public async Task RouteAsync(EventEntity evt)
    {
        if (_handlers.TryGetValue(evt.Name, out var handlerTypes))
        {
            var tasks = handlerTypes.Select(async type =>
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var handler = (IEventHandler)scope.ServiceProvider.GetRequiredService(type);
                    await handler.HandleAsync(evt);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Router] Handler {type.Name} failed: {ex}");
                }
            });

            await Task.WhenAll(tasks);
        }
        else
        {
            Console.WriteLine($"[Router] No handlers registered for event: {evt.Name}");
        }
    }

}