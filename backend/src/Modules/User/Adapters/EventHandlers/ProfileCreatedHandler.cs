namespace User.Adapters.EventHandlers;

using Common.Services.EventBroker.Core.Ports;
using Common.Services.EventBroker.Core.Entities;
using User.Core.DTOs;
using User.Core.Ports.Services;

public class ProfileCreatedHandler : IEventHandler
{
    public string EventName => "ProfileCreated";
    
    private readonly ISetProfileIdService _setProfileIdService;

    public ProfileCreatedHandler(ISetProfileIdService setProfileIdService)
    {
        _setProfileIdService = setProfileIdService;
    }
    
    public async Task HandleAsync(EventEntity eventEntity)
    {
        
        var payload = eventEntity.DeserializedRecord!;
        
        try
        { 
            var userId = Guid.Parse(payload["userId"].ToString()!);
            var profileId = Guid.Parse(payload["id"].ToString()!);
            
            await _setProfileIdService.SetProfileIdAsync(userId, profileId);
        }
        catch (Exception err)
        {
            Console.WriteLine(err);
            throw;
        }
    }
}