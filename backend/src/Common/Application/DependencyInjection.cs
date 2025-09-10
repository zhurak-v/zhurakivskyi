namespace Common.Application;

using Common.Services.EventBroker.Core.Ports;
using Common.Services.EventBroker.Infrastructure;
using Common.Utilities.PasswordHash;
using Common.Utilities.AvroPayload;
using Common.Utilities.SchemaLoader;
using Microsoft.Extensions.DependencyInjection;

public static class CommonServices
{
    public static void AddCommonServices(this IServiceCollection services)
    {
        services.AddSingleton<IEventHandlerRouter, EventHandlerRouter>();
        services.AddSingleton<IEventBroker>(sp =>
            new KafkaEventBroker(
                bootstrapServers: "kafka-broker:9092",
                schemaRegistryUrl: "http://schema-registry:8081",
                scopeFactory: sp.GetRequiredService<IServiceScopeFactory>()
            )
        );
        services.AddHostedService<KafkaConsumerService>();

        services.AddSingleton<ISchemaRegistry>(sp =>
            new KafkaSchemaRegistry("http://schema-registry:8081")
        );
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IPayloadBuilder, PayloadBuilder>();
        services.AddScoped<IAvroSchemaLoader, AvroSchemaLoader>();
    }
}
