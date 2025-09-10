namespace Auth.Core.Entities;

using Common.Entities;

public class AuthRegisterEntity : BaseEntity
{
    public string Email { get; set; }
    public string Password { get; set; }
    
    public AuthRegisterEntity(string email, string password)
        : base()
    {
        this.Email = email;
        this.Password = password;
    }
}