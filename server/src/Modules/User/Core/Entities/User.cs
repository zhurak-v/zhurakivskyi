namespace User.Core.Entities;

using Common.Entities;
using User.Core.Enums;
using User.Core.Ports.Entities;
public class UserEntity : BaseEntity, IUserEntity
{
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }

    public required Guid ProfileId { get; set; }

    public UserEntity(string email, string password, UserRole role = UserRole.REGULAR)
        : base()
    {
        this.Email = email;
        this.Password = password;
        this.Role = role;
    }
}
