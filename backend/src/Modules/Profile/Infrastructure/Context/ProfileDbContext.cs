namespace Profile.Infrastructure.Context;

using Microsoft.EntityFrameworkCore;
using Profile.Core.Entities;
using Profile.Infrastructure.Configuration;
using Common.Services.EventBroker.Core.Entities;

public class ProfileDbContext : DbContext
{
    public ProfileDbContext(DbContextOptions<ProfileDbContext> options)
        : base(options) { }

    public DbSet<ProfileEntity> Profiles { get; set; }
    public DbSet<EventEntity> EventEntities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProfileConfiguration());
        modelBuilder.Entity<EventEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Ignore(e => e.DeserializedRecord);
        });
        base.OnModelCreating(modelBuilder);
    }
}