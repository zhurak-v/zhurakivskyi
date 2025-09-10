namespace User.Application;

using Common.Services.EventBroker.Infrastructure;
using Common.Services.EventBroker.Core.Ports;

using User.Adapters.EventHandlers;
using User.Core.Ports.Services;
using User.Core.Ports.Repository;
using User.Core.Ports.Transaction;
using User.Core.Ports.Outbox;
using User.Core.Services;
using User.Infrastructure.Repository;
using User.Infrastructure.Context;
using User.Infrastructure.Transaction;
using User.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class UserServices
{
    public static void AddUserServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<UserDbContext>(options =>
            options.UseNpgsql(connectionString)
        );

        services.AddScoped<ICreateUserService, CreateUserService>();
        services.AddScoped<ISetProfileIdService, SetProfileIdService>();
        services.AddScoped<IUserTransaction, UserTransaction>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserOutboxRepository, UserOutboxRepository>();
        services.AddScoped<IUserEventWorker, UserEventWorker>();

        services.AddScoped<AuthRegisteredHandler>();
        services.AddScoped<IEventHandler>(sp => sp.GetRequiredService<AuthRegisteredHandler>());
        
        services.AddScoped<ProfileCreatedHandler>();
        services.AddScoped<IEventHandler>(sp => sp.GetRequiredService<ProfileCreatedHandler>());

        services.AddHostedService(sp =>
            new OutboxListener<IUserEventWorker>(
                sp.GetRequiredService<IServiceScopeFactory>(),
                connectionString,
                "user_outbox_channel"
            )
        );
    }
}