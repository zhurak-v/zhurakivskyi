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

public class CreateUserService : ICreateUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserTransaction _userTransaction;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ISchemaRegistry _schemaRegistry;
    private readonly IUserOutboxRepository _userOutboxRepository;
    private readonly IPayloadBuilder _payloadBuilder;
    private readonly IAvroSchemaLoader _avroSchemaLoader;
    
    public CreateUserService(
        IUserRepository userRepository,
        IUserTransaction userTransaction,
        IPasswordHasher passwordHasher,
        ISchemaRegistry schemaRegistry,
        IUserOutboxRepository userOutboxRepository,
        IPayloadBuilder payloadBuilder,
        IAvroSchemaLoader avroSchemaLoader
    )
    {
        _userRepository = userRepository;
        _userTransaction = userTransaction;
        _passwordHasher = passwordHasher;
        _schemaRegistry = schemaRegistry;
        _userOutboxRepository = userOutboxRepository;
        _payloadBuilder = payloadBuilder;
        _avroSchemaLoader = avroSchemaLoader;
    }

    public async Task<UserEntity> CreateUserAsync(CreateUserDto dto)
    {
        var hashedPassword = _passwordHasher.HashPassword(dto.Password);
        var user = new UserEntity(dto.Email, hashedPassword, UserRole.REGULAR);
        
        var schemaJson = _avroSchemaLoader.LoadSchema("UserCreated");
        var schemaId = await _schemaRegistry.RegisterSchemaAsync("UserCreated-schema", schemaJson);
        
        var payload = _payloadBuilder.Serialize(schemaJson, record =>
        {
            record.Add("id", user.Id);
            record.Add("email", user.Email);
            record.Add("password", user.Password);
            record.Add("role", user.Role);
            record.Add("profileId", user.ProfileId);
        });
        
        var evt = new EventEntity(
            topic: "user-events",
            name: "UserCreated",
            payload: payload,
            schemaId: schemaId
        );
        
        await _userTransaction.ExecuteAsync(async () =>
        {
            await _userOutboxRepository.CreateAsync(evt);
            await _userRepository.CreateAsync(user);
        });
        
        return user;
    }
}