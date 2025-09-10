namespace Auth.Adapters.Http;

using Microsoft.AspNetCore.Mvc;
using Auth.Core.Ports.Services;
using Auth.Core.DTOs;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthRegister _authRegisterService;
    
    public AuthController(IAuthRegister authRegisterService)
    {
        _authRegisterService = authRegisterService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> SendOrder([FromBody] AuthRegisterDto request)
    {
        await _authRegisterService.RegisterAsync(new AuthRegisterDto
        {
            Email = request.Email,
            Password = request.Password
        });
        
        return Ok(new { response = "Register success" });
    }
}

