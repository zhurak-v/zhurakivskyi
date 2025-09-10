namespace User.Core.Services;

using User.Core.Ports.Services;
using User.Core.Entities;
using User.Core.DTOs;
using User.Core.Enums;
using User.Core.Ports.Repository;
using User.Core.Ports.Transaction;

using Common.Utilities.PasswordHash;
using Common.Utilities.AvroPayload;
using Common.Utilities.SchemaLoader;
using Common.Services.EventBroker.Core.Entities;
using Common.Services.EventBroker.Core.Ports;

public class SetProfileIdService : ISetProfileIdService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserTransaction _userTransaction;
    private readonly ISchemaRegistry _schemaRegistry;
    private readonly IUserOutboxRepository _userOutboxRepository;
    private readonly IPayloadBuilder _payloadBuilder;
    private readonly IAvroSchemaLoader _avroSchemaLoader;
    
    public SetProfileIdService(
        IUserRepository userRepository,
        IUserTransaction userTransaction,
        ISchemaRegistry schemaRegistry,
        IUserOutboxRepository userOutboxRepository,
        IPayloadBuilder payloadBuilder,
        IAvroSchemaLoader avroSchemaLoader
    )
    {
        _userRepository = userRepository;
        _userTransaction = userTransaction;
        _schemaRegistry = schemaRegistry;
        _userOutboxRepository = userOutboxRepository;
        _payloadBuilder = payloadBuilder;
        _avroSchemaLoader = avroSchemaLoader;
    }

    public async Task SetProfileIdAsync(Guid userId, Guid profileId)
    {
        await _userTransaction.ExecuteAsync(async () =>
        {
            await _userRepository.UpdateAsync(userId, user =>
            {
                user.ProfileId = profileId;
            });
        });
    }
}