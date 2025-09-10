namespace Common.Services.EventBroker.Core.Ports;

using System;
using System.Threading.Tasks;
using Common.Services.EventBroker.Core.Entities;

public interface IEventHandlerRouter
{
    Task RouteAsync(EventEntity evt);
}