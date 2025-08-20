namespace User.Core.Services;

using User.Core.Entities;
using User.Core.Ports.Repository;
using User.Core.Ports.Services;

public class FindUserByEmailService : IFindUserByEmailService
{
    private readonly IUserRepository _userRepository;

    public FindUserByEmailService(IUserRepository userRepository)
    {
        this._userRepository = userRepository;
    }

    public async Task<UserEntity> FindUserByEmail(string email)
    {
        var user = await this._userRepository.FindByEmail(email);
        
        if (user is null)
            throw new InvalidOperationException("User with this email does not exist.");
        
        return user;
    }
    
}