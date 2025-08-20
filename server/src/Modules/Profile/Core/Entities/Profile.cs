namespace Profile.Core.Entities;

using Common.Entities;

public sealed class ProfileEntity : BaseEntity
{
    public string? NickName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ProfilePicture { get; set; }

    public Guid UserId { get; set; }

    public ProfileEntity(
        Guid userId, 
        string? nickName = null, 
        string? firstName = null, 
        string? lastName = null, 
        string? profilePicture = null)
    {
        UserId = userId;
        NickName = nickName;
        FirstName = firstName;
        LastName = lastName;
        ProfilePicture = profilePicture;
    }
}