namespace Profile.Infrastructure.Repository.Context;

using Microsoft.EntityFrameworkCore;
using Profile.Core.Entities;
using Profile.Infrastructure.Repository.Configuration;

public class ProfileDbContext : DbContext
{
    public ProfileDbContext(DbContextOptions<ProfileDbContext> options)
        : base(options) { }

    public DbSet<ProfileEntity> Profiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProfileConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}