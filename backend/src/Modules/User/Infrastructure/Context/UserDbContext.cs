namespace User.Infrastructure.Context;

using Microsoft.EntityFrameworkCore;
using User.Core.Entities;
using User.Infrastructure.Configuration;
using Common.Services.EventBroker.Core.Entities;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options) { }

    public DbSet<UserEntity> Users { get; set; }
    public DbSet<EventEntity> EventEntities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.Entity<EventEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Ignore(e => e.DeserializedRecord);
        });
        base.OnModelCreating(modelBuilder);
    }
}