namespace User.Core.Ports.Services;

public interface ISetProfileIdService
{
    Task SetProfileIdAsync(Guid userId, Guid profileId);
}