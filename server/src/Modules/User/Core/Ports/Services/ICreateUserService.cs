namespace User.Core.Ports.Services;

using User.Core.Entities;
using User.Core.DTOs;

public interface ICreateUserService
{
    Task<UserEntity> CreateUserAsync(CreateUserDto dto);
}