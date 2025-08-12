namespace Core.Entity;

public class ProfileEntity : BaseEntity
{
    public string? NickName { get; set; }
    public string? FullName { get; set; }
    public string? ProfilePicture { get; set; }

    public required Guid UserId { get; set; }
    public UserEntity? User { get; set; }

    public ProfileEntity()
        : this(null, null)
    { }

    public ProfileEntity(string? fullName, string? profilePicture)
        : base()
    {
        this.FullName = fullName;
        this.ProfilePicture = profilePicture;
    }
}