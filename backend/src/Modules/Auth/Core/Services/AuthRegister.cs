namespace Auth.Core.Services;

using System.Threading.Tasks;

using Auth.Core.Ports.Services;
using Auth.Core.Entities;
using Auth.Core.DTOs;
using Auth.Core.Ports.Repository;
using Auth.Core.Ports.Transaction;

using Common.Services.EventBroker.Core.Ports;
using Common.Services.EventBroker.Core.Entities;
using Common.Utilities.AvroPayload;
using Common.Utilities.SchemaLoader;

public class AuthRegister : IAuthRegister
{
    private readonly ISchemaRegistry _schemaRegistry;
    private readonly IAuthOutboxRepository _authOutboxRepository;
    private readonly IAuthTransaction _authTransaction;
    private readonly IPayloadBuilder _payloadBuilder;
    private readonly IAvroSchemaLoader _avroSchemaLoader;

    public AuthRegister(
        ISchemaRegistry schemaRegistry,
        IAuthOutboxRepository authOutboxRepository,
        IAuthTransaction authTransaction,
        IPayloadBuilder payloadBuilder,
        IAvroSchemaLoader avroSchemaLoader
    )
    {
        _schemaRegistry = schemaRegistry;
        _authOutboxRepository = authOutboxRepository;
        _authTransaction = authTransaction;
        _payloadBuilder = payloadBuilder;
        _avroSchemaLoader = avroSchemaLoader;
    }

    public async Task<AuthRegisterEntity> RegisterAsync(AuthRegisterDto dto)
    {
        var schemaJson = _avroSchemaLoader.LoadSchema("AuthRegistered");
        var schemaId = await _schemaRegistry.RegisterSchemaAsync("AuthRegistered-schema", schemaJson);

        var payload = _payloadBuilder.Serialize(schemaJson, record =>
        {
            record.Add("email", dto.Email);
            record.Add("password", dto.Password);
        });

        var evt = new EventEntity(
            topic: "auth-events",
            name: "AuthRegistered",
            payload: payload,
            schemaId: schemaId
        );

        await _authTransaction.ExecuteAsync(async () =>
        {
            Console.WriteLine("Sending to Auth Outbox");
            await _authOutboxRepository.CreateAsync(evt);
        });

        return new AuthRegisterEntity(dto.Email, dto.Password);
    }
}
