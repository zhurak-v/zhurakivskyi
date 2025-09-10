namespace Auth.Application;

using Common.Services.EventBroker.Infrastructure;
using Auth.Core.Ports.Services;
using Auth.Core.Ports.Transaction;
using Auth.Core.Ports.Outbox;
using Auth.Core.Ports.Repository;
using Auth.Core.Services;
using Auth.Infrastructure.Context;
using Auth.Infrastructure.Transaction;
using Auth.Infrastructure.Repository;
using Auth.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class AuthServices
{
    public static void AddAuthServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AuthDbContext>(options =>
            options.UseNpgsql(connectionString)
        );

        services.AddScoped<IAuthRegister, AuthRegister>();
        services.AddScoped<IAuthTransaction, AuthTransaction>();
        services.AddScoped<IAuthRegisterRepository, AuthRepository>();
        services.AddScoped<IAuthOutboxRepository, AuthOutboxRepository>();
        services.AddScoped<IAuthEventWorker, AuthEventWorker>();

        services.AddHostedService(sp =>
            new OutboxListener<IAuthEventWorker>(
                sp.GetRequiredService<IServiceScopeFactory>(),
                connectionString,
                "auth_outbox_channel"
            )
        );
    }
}