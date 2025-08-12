namespace Core.Entity;

public class UserEntity : BaseEntity
{
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }

    public required ProfileEntity Profile { get; set; }

    public UserEntity(string email, string password, UserRole role = UserRole.USER)
        : base()
    {
        this.Email = email;
        this.Password = password;
        this.Role = role;
    }
}

public enum UserRole
{
    USER, ADMIN
}