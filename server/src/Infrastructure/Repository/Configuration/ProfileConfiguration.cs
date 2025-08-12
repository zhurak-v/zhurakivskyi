namespace Infrastructure.Repository.Configuration;

using Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProfileConfiguration : IEntityTypeConfiguration<ProfileEntity>
{
    public void Configure(EntityTypeBuilder<ProfileEntity> builder)
    {
        builder.HasKey(p => p.Id);

        builder
            .HasOne(p => p.User)
            .WithOne(u => u.Profile)
            .HasForeignKey<ProfileEntity>(p => p.UserId);
    }
}