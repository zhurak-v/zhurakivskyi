namespace User.Core.Entities;

using Common.Entities;
using User.Core.Enums;

public class UserEntity : BaseEntity
{
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }

    public Guid? ProfileId { get; set; }

    public UserEntity(string email, string password, UserRole role)
        : base()
    {
        this.Email = email;
        this.Password = password;
        this.Role = role;
    }
}
