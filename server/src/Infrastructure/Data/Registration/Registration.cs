namespace Infrastructure.Data.Registration;

using Infrastructure.Data.Repository;
using Microsoft.Extensions.DependencyInjection;
using Profile.Core.Ports.Repository;
using User.Core.Ports.Repository;

public static class RepositoryRegistration
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProfileRepository, ProfileRepository>();

        return services;
    }
}