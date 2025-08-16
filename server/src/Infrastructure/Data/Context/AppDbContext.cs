namespace Infrastructure.Data.Context;

using Infrastructure.Data.Configuration;
using Microsoft.EntityFrameworkCore;
using Profile.Core.Entities;
using User.Core.Entities;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<UserEntity> Users { get; set; }
    public DbSet<ProfileEntity> Profiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new ProfileConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}