namespace User.Core.Services;

using User.Core.Ports.Services;
using User.Core.Entities;
using User.Core.Enums;
using User.Core.Ports.Repository;
using User.Core.DTOs;
using Common.Utilities.PasswordHash;

public class CreateUserService :  ICreateUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        this._userRepository = userRepository;
        this._passwordHasher = passwordHasher;
    }

    public async Task<UserEntity> CreateUserAsync(CreateUserDto dto)
    {
        var existsUser = await this._userRepository.ExistsByEmailAsync(dto.Email);
        var hashedPassword = this._passwordHasher.HashPassword(dto.Password);

        if (existsUser)
            throw new InvalidOperationException("User with this email already exists.");

        var user = new UserEntity(dto.Email, hashedPassword, dto.Role ?? UserRole.REGULAR);
        await this._userRepository.CreateAsync(user);

        return user;
    }
}