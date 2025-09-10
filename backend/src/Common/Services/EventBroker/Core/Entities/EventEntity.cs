namespace Common.Services.EventBroker.Core.Entities;

using Common.Entities;
using Avro.Generic;

public class EventEntity : BaseEntity
{
    public string Topic { get; private set; }
    public string Name { get; private set; }
    public byte[] Payload { get; private set; }
    public int SchemaId { get; private set; }
    
    public GenericRecord? DeserializedRecord { get; set; }
    public EventStatus Status { get; private set; } = EventStatus.New;

    public void MarkProcessed() => Status = EventStatus.Processed;
    public void MarkFailed() => Status = EventStatus.Failed;

    public EventEntity(string topic, string name, byte[] payload, int schemaId)
    {
        SchemaId = schemaId;
        Topic = topic;
        Name = name;
        Payload = payload;
    }

    protected EventEntity() { }
}

public enum EventStatus
{
    New,
    Processed,
    Failed
}