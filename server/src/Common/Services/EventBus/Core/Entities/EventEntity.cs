namespace Common.Services.EventBus.Core.Entities;

using System.Text.Json;
using Common.Entities;

public class EventEntity<TPayload> : BaseEntity
{
    public string EventTopic { get; set; }
    public string EventName { get; set; }
    
    public string Payload { get; private set; }

    public EventEntity(string eventTopic, string eventName, TPayload payload)
    {
        EventTopic = eventTopic;
        EventName = eventName;
        this.SetPayload(payload);
    }

    private void SetPayload(
        TPayload payload)
    {
        Payload = JsonSerializer.Serialize(payload, _options);
    }

    public TPayload GetPayload()
    {
        return JsonSerializer.Deserialize<TPayload>(Payload, _options)!;
    }
    
    private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };
}