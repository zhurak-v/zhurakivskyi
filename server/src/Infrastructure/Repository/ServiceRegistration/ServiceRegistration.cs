namespace Infrastructure.Repository.ServiceRegistration;

using Infrastructure.Repository.Configuration;
using Infrastructure.Repository.Transaction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ports.Repository;

public static class RepositoryServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<RepositoryDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProfileRepository, ProfileRepository>();

        services.AddScoped<ITransaction, Transaction>();

        return services;
    }
}