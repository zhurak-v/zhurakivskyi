﻿namespace Common.Services.EventBroker.Infrastructure;

using Common.Data.Repository;
using Common.Services.EventBroker.Core.Entities;
using Common.Services.EventBroker.Core.Ports;
using Microsoft.EntityFrameworkCore;

public class EventRepository<TContext> : BaseRepository<EventEntity, TContext>, IEventRepository
    where TContext : DbContext
{
    public EventRepository(TContext context) : base(context) { }

    public async Task AddRangeAsync(IEnumerable<EventEntity> events)
    {
        await DbSet.AddRangeAsync(events);
    }

    public async Task<IEnumerable<EventEntity>> GetByCreatedAtRangeAsync(DateTime from, DateTime to)
    {
        return await DbSet.Where(e => e.CreatedAt >= from && e.CreatedAt <= to).ToListAsync();
    }

    public async Task<IEnumerable<EventEntity>> GetByNameAsync(string name)
    {
        return await DbSet.Where(e => e.Name == name).ToListAsync();
    }

    public async Task<IEnumerable<EventEntity>> GetByStatusAsync(EventStatus status)
    {
        return await DbSet.Where(e => e.Status == status).ToListAsync();
    }

    public async Task<IEnumerable<EventEntity>> GetByTopicAsync(string topic)
    {
        return await DbSet.Where(e => e.Topic == topic).ToListAsync();
    }

    public async Task MarkFailedAsync(EventEntity entity)
    {
        await UpdateAsync(entity.Id, evt =>
        {
            evt.MarkFailed();
        });
        await CommitAsync();
    }

    public async Task MarkProcessedAsync(EventEntity entity)
    {
        await UpdateAsync(entity.Id, evt =>
        {
            evt.MarkProcessed();
        });
        await CommitAsync();
    }
}