namespace User.Adapters.EventHandlers;

using Common.Services.EventBroker.Core.Ports;
using Common.Services.EventBroker.Core.Entities;
using User.Core.DTOs;
using User.Core.Ports.Services;

public class AuthRegisteredHandler : IEventHandler
{
    public string EventName => "AuthRegistered";
    
    private readonly ICreateUserService _createUserService;

    public AuthRegisteredHandler(ICreateUserService createUserService)
    {
        _createUserService = createUserService;
    }
    
    public async Task HandleAsync(EventEntity eventEntity)
    {
        
        var payload = eventEntity.DeserializedRecord!;
        
        try
        { 
            var email = (string)payload["email"];
            var password = (string)payload["password"];
            
            await _createUserService.CreateUserAsync(new CreateUserDto
            {
                Email = email,
                Password = password,
            });

        }
        catch (Exception err)
        {
            Console.WriteLine(err);
            throw;
        }
    }
}