namespace Profile.Infrastructure.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profile.Core.Entities;

public class ProfileConfiguration : IEntityTypeConfiguration<ProfileEntity>
{
    public void Configure(EntityTypeBuilder<ProfileEntity> builder)
    {
        builder.HasKey(p => p.Id);

        builder
            .Property(p => p.UserId)
            .IsRequired();

        builder
            .Property(p => p.FirstName)
            .HasMaxLength(100)
            .IsRequired(false);

        builder
            .Property(p => p.LastName)
            .HasMaxLength(100)
            .IsRequired(false);

        builder
            .Property(p => p.Avatar)
            .HasMaxLength(250)
            .IsRequired(false);

        builder
            .Property(p => p.PhoneNumber)
            .HasMaxLength(20)
            .IsRequired(false);

        builder
            .Property(p => p.NickName)
            .HasMaxLength(50)
            .IsRequired(false);

        builder
            .HasIndex(p => p.NickName)
            .IsUnique()
            .HasFilter("\"NickName\" IS NOT NULL");

        builder
            .HasIndex(p => p.UserId)
            .IsUnique();

        builder.HasIndex(p => p.FirstName);
        builder.HasIndex(p => p.LastName);
    }
}