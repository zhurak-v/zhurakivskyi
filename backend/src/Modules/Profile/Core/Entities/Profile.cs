namespace Profile.Core.Entities;

using System;
using Common.Entities;

public class ProfileEntity : BaseEntity
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Avatar { get; set; }
    public string? PhoneNumber { get; set; }
    public string? NickName { get; set; }

    public Guid UserId { get; set; }

    public ProfileEntity(
        Guid userId,
        string? firstName = null, 
        string? lastName = null, 
        string? avatar = null, 
        string? phoneNumber = null,
        string? nickName = null
        )
        : base()
    {
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
        Avatar = avatar;
        PhoneNumber = phoneNumber;
        NickName = nickName;
    }
}