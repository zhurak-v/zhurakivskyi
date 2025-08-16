namespace User.Core.Ports.Entities;

using Common.Entities;
using User.Core.Enums;

public interface IUserEntity : IBaseEntity
{
    string Email { get; set; }
    string Password { get; set; }
    UserRole Role { get; set; }
}