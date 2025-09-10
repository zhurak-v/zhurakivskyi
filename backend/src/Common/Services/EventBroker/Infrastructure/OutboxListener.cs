namespace Common.Services.EventBroker.Infrastructure;

using Npgsql;
using Common.Services.EventBroker.Core.Ports;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

public class OutboxListener<TEventWorker> : BackgroundService, IOutboxListener
    where TEventWorker :  IEventWorker
{
    private readonly string _connectionString;
    private readonly string _channelName;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public OutboxListener(
        IServiceScopeFactory scopeFactory,
        string connectionString,
        string channelName)
    {
        _scopeFactory = scopeFactory;
        _connectionString = connectionString;
        _channelName = channelName;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync(stoppingToken);

        conn.Notification += async (_, e) =>
        {
            await ProcessEventsAsync(stoppingToken);
        };

        await using (var cmd = new NpgsqlCommand($"LISTEN {_channelName};", conn))
        {
            await cmd.ExecuteNonQueryAsync(stoppingToken);
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            await conn.WaitAsync(stoppingToken);
            await ProcessEventsAsync(stoppingToken);
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }

    private async Task ProcessEventsAsync(CancellationToken stoppingToken)
    {
        if (!await _semaphore.WaitAsync(0, stoppingToken))
        {
            return;
        }

        try
        {
            using var scope = _scopeFactory.CreateScope();
            var worker = scope.ServiceProvider.GetRequiredService<TEventWorker>();
            await worker.ProcessEventAsync(stoppingToken);
        }
        finally
        {
            _semaphore.Release();
        }
    }
}
