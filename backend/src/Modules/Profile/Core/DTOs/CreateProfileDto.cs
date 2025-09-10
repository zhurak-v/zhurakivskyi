namespace Profile.Core.DTOs;

public class CreateProfileDto
{
    public Guid UserId { get; set; }
    
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Avatar { get; set; }
    public string? PhoneNumber { get; set; }
    public string? NickName { get; set; }
}