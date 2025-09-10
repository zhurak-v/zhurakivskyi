namespace Auth.Core.Ports.Services;

using Auth.Core.Entities;
using Auth.Core.DTOs;

public interface IAuthRegister
{
    Task<AuthRegisterEntity> RegisterAsync(AuthRegisterDto dto);
}