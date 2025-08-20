namespace User.Core.DTOs;

using User.Core.Enums;

public class CreateUserDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRole? Role { get; set; }
}