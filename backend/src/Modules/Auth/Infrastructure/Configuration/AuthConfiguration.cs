namespace Auth.Infrastructure.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Auth.Core.Entities;

public class AuthConfiguration : IEntityTypeConfiguration<AuthRegisterEntity>
{
    public void Configure(EntityTypeBuilder<AuthRegisterEntity> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.Password)
            .IsRequired()
            .HasMaxLength(255);
    }
}
