namespace Profile.Core.Ports.Services;

using Profile.Core.Entities;
using Profile.Core.DTOs;

public interface ICreateProfileService
{
    Task<ProfileEntity> CreateProfileAsync(CreateProfileDto dto);
}