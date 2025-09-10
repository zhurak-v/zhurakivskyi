namespace Profile.Adapters.EventHandlers;

using Common.Services.EventBroker.Core.Ports;
using Common.Services.EventBroker.Core.Entities;
using Profile.Core.DTOs;
using Profile.Core.Ports.Services;

public class UserCreatedHandler : IEventHandler
{
    public string EventName => "UserCreated";
    
    private readonly ICreateProfileService _createProfileService;

    public UserCreatedHandler(ICreateProfileService createProfileService)
    {
        _createProfileService = createProfileService;
    }
    
    public async Task HandleAsync(EventEntity eventEntity)
    {
        
        var payload = eventEntity.DeserializedRecord!;
        
        try
        { 
            var userId = Guid.Parse(payload["id"].ToString()!);
            
            await _createProfileService.CreateProfileAsync(new CreateProfileDto
            {
                UserId = userId,
                });

        }
        catch (Exception err)
        {
            Console.WriteLine(err);
            throw;
        }
    }
}