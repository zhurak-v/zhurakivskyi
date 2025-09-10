namespace Profile.Application;

using Common.Services.EventBroker.Infrastructure;
using Common.Services.EventBroker.Core.Ports;

using Profile.Adapters.EventHandlers;
using Profile.Core.Ports.Services;
using Profile.Core.Ports.Repository;
using Profile.Core.Ports.Transaction;
using Profile.Core.Ports.Outbox;
using Profile.Core.Services;
using Profile.Infrastructure.Repository;
using Profile.Infrastructure.Context;
using Profile.Infrastructure.Transaction;
using Profile.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class ProfileServices
{
    public static void AddProfileServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ProfileDbContext>(options =>
            options.UseNpgsql(connectionString)
        );

        services.AddScoped<ICreateProfileService, CreateProfileService>();
        services.AddScoped<IProfileTransaction, ProfileTransaction>();
        services.AddScoped<IProfileRepository, ProfileRepository>();
        services.AddScoped<IProfileOutboxRepository, ProfileOutboxRepository>();
        services.AddScoped<IProfileEventWorker, ProfileEventWorker>();

        services.AddScoped<UserCreatedHandler>();
        services.AddScoped<IEventHandler>(sp => sp.GetRequiredService<UserCreatedHandler>());

        services.AddHostedService(sp =>
            new OutboxListener<IProfileEventWorker>(
                sp.GetRequiredService<IServiceScopeFactory>(),
                connectionString,
                "profile_outbox_channel"
            )
        );
    }
}