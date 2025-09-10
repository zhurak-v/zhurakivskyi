namespace Auth.Infrastructure.Context;

using Microsoft.EntityFrameworkCore;
using Auth.Core.Entities;
using Auth.Infrastructure.Configuration;
using Common.Services.EventBroker.Core.Entities;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options)
        : base(options) { }

    public DbSet<AuthRegisterEntity> Auths { get; set; }
    public DbSet<EventEntity> EventEntities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AuthConfiguration());
        modelBuilder.Entity<EventEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Ignore(e => e.DeserializedRecord);
        });
        base.OnModelCreating(modelBuilder);
    }
}