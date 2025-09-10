namespace Profile.Core.Services;

using Profile.Core.Ports.Services;
using Profile.Core.Entities;
using Profile.Core.DTOs;
using Profile.Core.Ports.Repository;
using Profile.Core.Ports.Transaction;

using Common.Utilities.PasswordHash;
using Common.Utilities.AvroPayload;
using Common.Utilities.SchemaLoader;
using Common.Services.EventBroker.Core.Entities;
using Common.Services.EventBroker.Core.Ports;

public class CreateProfileService : ICreateProfileService
{
    private readonly IProfileRepository _profileRepository;
    private readonly IProfileTransaction _profileTransaction;
    private readonly ISchemaRegistry _schemaRegistry;
    private readonly IProfileOutboxRepository _profileOutboxRepository;
    private readonly IPayloadBuilder _payloadBuilder;
    private readonly IAvroSchemaLoader _avroSchemaLoader;
    
    public CreateProfileService(
        IProfileRepository profileRepository,
        IProfileTransaction profileTransaction,
        ISchemaRegistry schemaRegistry,
        IProfileOutboxRepository profileOutboxRepository,
        IPayloadBuilder payloadBuilder,
        IAvroSchemaLoader avroSchemaLoader
    )
    {
        _profileRepository = profileRepository;
        _profileTransaction = profileTransaction;
        _schemaRegistry = schemaRegistry;
        _profileOutboxRepository = profileOutboxRepository;
        _payloadBuilder = payloadBuilder;
        _avroSchemaLoader = avroSchemaLoader;
    }

    public async Task<ProfileEntity> CreateProfileAsync(CreateProfileDto dto)
    {
        var profile = new ProfileEntity(dto.UserId, dto.FirstName, dto.LastName, dto.Avatar, dto.PhoneNumber, dto.NickName);
        
        var schemaJson = _avroSchemaLoader.LoadSchema("ProfileCreated");
        var schemaId = await _schemaRegistry.RegisterSchemaAsync("ProfileCreated-schema", schemaJson);
        
        var payload = _payloadBuilder.Serialize(schemaJson, record =>
        {
            record.Add("id", profile.Id.ToString());
            record.Add("firstName", profile.FirstName ?? null);
            record.Add("lastName", profile.LastName ?? null);
            record.Add("avatar", profile.Avatar ?? null);
            record.Add("phoneNumber", profile.PhoneNumber ?? null);
            record.Add("nickName", profile.NickName ?? null);
            record.Add("userId", profile.UserId.ToString());
        });
        
        var evt = new EventEntity(
            topic: "profile-events",
            name: "ProfileCreated",
            payload: payload,
            schemaId: schemaId
        );
        
        await _profileTransaction.ExecuteAsync(async () =>
        {
            await _profileOutboxRepository.CreateAsync(evt);
            await _profileRepository.CreateAsync(profile);
        });
        
        return profile;
    }
    
}