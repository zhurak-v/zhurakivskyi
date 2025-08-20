namespace User.Core.Ports.Services;

using User.Core.Entities;

public interface IFindUserByEmailService
{
    Task<UserEntity> FindUserByEmail(string email);
}