namespace Profile.Core.Ports.Entities;

using Common.Entities;

public interface IProfileEntity : IBaseEntity
{
    string? NickName { get; set; }
    string? FirstName { get; set; }
    string? LastName { get; set; }
    string? ProfilePicture { get; set; }
}