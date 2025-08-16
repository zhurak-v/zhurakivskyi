namespace Profile.Core.Entities;

using Common.Entities;
using Profile.Core.Ports.Entities;

public sealed class ProfileEntity : BaseEntity, IProfileEntity
{
    public string? NickName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ProfilePicture { get; set; }

    public required Guid UserId { get; set; }

    public ProfileEntity()
        : this(null, null, null, null)
    { }

    public ProfileEntity(string? nickName, string? firstName, string? lastName, string? profilePicture)
        : base()
    {
        this.NickName = nickName;
        this.FirstName = firstName;
        this.LastName = lastName;
        this.ProfilePicture = profilePicture;
    }
}