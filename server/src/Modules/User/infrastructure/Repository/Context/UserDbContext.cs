namespace User.Infrastructure.Repository.Context;

using Microsoft.EntityFrameworkCore;
using User.Core.Entities;
using User.Infrastructure.Repository.Configuration;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options) { }

    public DbSet<UserEntity> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}