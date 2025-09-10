namespace Common.Services.EventBroker.Infrastructure;

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Avro;
using Common.Services.EventBroker.Core.Entities;
using Common.Services.EventBroker.Core.Ports;
using Microsoft.Extensions.DependencyInjection;
using Common.Utilities.AvroPayload;

public class KafkaEventBroker : IEventBroker, IAsyncDisposable
{
    private readonly IProducer<string, byte[]> _producer;
    private readonly CachedSchemaRegistryClient _schemaRegistry;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly string _bootstrapServers;
    private readonly ConcurrentDictionary<int, RecordSchema> _schemaCache = new();
    private readonly ConcurrentDictionary<string, Task> _consumerTasks = new();
    private readonly CancellationTokenSource _cts = new();
    private readonly Channel<EventEntity> _channel;

    public KafkaEventBroker(
        string bootstrapServers,
        string schemaRegistryUrl,
        IServiceScopeFactory scopeFactory)
    {
        _bootstrapServers = bootstrapServers;
        _scopeFactory = scopeFactory;

        _producer = new ProducerBuilder<string, byte[]>(new ProducerConfig
        {
            BootstrapServers = bootstrapServers,
            LingerMs = 5,
            BatchSize = 128 * 1024,
            CompressionType = CompressionType.Zstd,
            Acks = Acks.Leader
        }).Build();

        _schemaRegistry = new CachedSchemaRegistryClient(new SchemaRegistryConfig
        {
            Url = schemaRegistryUrl
        });

        _channel = Channel.CreateUnbounded<EventEntity>(new UnboundedChannelOptions
        {
            SingleReader = false,
            SingleWriter = false,
            AllowSynchronousContinuations = false
        });

        _ = Task.Run(() => DispatchLoop(_cts.Token));
    }

    public async Task<bool> PublishAsync(EventEntity eventEntity)
    {
        try
        {
            var message = new Message<string, byte[]>
            {
                Key = eventEntity.Id.ToString(),
                Value = eventEntity.Payload,
                Headers = new Headers
                {
                    { "schemaId", BitConverter.GetBytes(eventEntity.SchemaId) },
                    { "eventName", System.Text.Encoding.UTF8.GetBytes(eventEntity.Name) }
                }
            };

            await _producer.ProduceAsync(eventEntity.Topic, message);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Kafka] Publish failed: {ex.Message}");
            return false;
        }
    }

    public Task SubscribeAsync(string topic, CancellationToken ct)
    {
        _consumerTasks.GetOrAdd(topic, _ => Task.Run(() => StartConsumerGroup(topic, ct), ct));
        return Task.CompletedTask;
    }

    private async Task StartConsumerGroup(string topic, CancellationToken ct)
    {
        int consumerCount = Math.Max(1, Environment.ProcessorCount / 2);
        var tasks = new Task[consumerCount];
        for (int i = 0; i < consumerCount; i++)
            tasks[i] = Task.Run(() => StartConsumerLoop(topic, ct), ct);

        await Task.WhenAll(tasks).ConfigureAwait(false);
    }

    private async Task StartConsumerLoop(string topic, CancellationToken ct)
    {
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = _bootstrapServers,
            GroupId = $"group-{topic}",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = true,
            FetchMinBytes = 1_048_576,
            FetchWaitMaxMs = 50
        };

        using var consumer = new ConsumerBuilder<string, byte[]>(consumerConfig).Build();
        consumer.Subscribe(topic);

        try
        {
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    var cr = consumer.Consume(ct);

                    if (!cr.Message.Headers.TryGetLastBytes("schemaId", out var schemaBytes)) continue;
                    if (!cr.Message.Headers.TryGetLastBytes("eventName", out var nameBytes)) continue;

                    var schemaId = BitConverter.ToInt32(schemaBytes);
                    var eventName = System.Text.Encoding.UTF8.GetString(nameBytes);

                    if (!_schemaCache.TryGetValue(schemaId, out var recordSchema))
                    {
                        var schemaFromRegistry = await _schemaRegistry.GetSchemaAsync(schemaId);
                        recordSchema = (RecordSchema)Avro.Schema.Parse(schemaFromRegistry.SchemaString);
                        _schemaCache[schemaId] = recordSchema;
                    }
                    
                    using var scope = _scopeFactory.CreateScope();
                    var payloadBuilder = scope.ServiceProvider.GetRequiredService<IPayloadBuilder>();
                    var deserializedRecord = payloadBuilder.Deserialize(recordSchema.ToString(), cr.Message.Value);

                    var evt = new EventEntity(topic, eventName, cr.Message.Value, schemaId)
                    {
                        DeserializedRecord = deserializedRecord
                    };
                    
                    await _channel.Writer.WriteAsync(evt, ct).ConfigureAwait(false);
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"[Kafka] Consume error: {e.Error.Reason}");
                }
            }
        }
        finally
        {
            consumer.Close();
        }
    }

    private async Task DispatchLoop(CancellationToken ct)
    {
        await foreach (var evt in _channel.Reader.ReadAllAsync(ct))
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var router = scope.ServiceProvider.GetRequiredService<IEventHandlerRouter>();
                    await router.RouteAsync(evt);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Kafka] Handler failed: {ex}");
                }
            }, ct);
        }
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            _cts.Cancel();
            await Task.WhenAll(_consumerTasks.Values).ConfigureAwait(false);
        }
        catch
        {
            Console.WriteLine($"[Kafka] Consumer cancelled");
        }
        finally
        {
            _producer.Flush();
            _producer.Dispose();
            _schemaRegistry.Dispose();
            _cts.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
