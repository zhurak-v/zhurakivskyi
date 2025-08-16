namespace Infrastructure.Data.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profile.Core.Entities;
using User.Core.Entities;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
       public void Configure(EntityTypeBuilder<UserEntity> builder)
       {
              builder.HasKey(u => u.Id);

              builder.Property(u => u.Email)
                     .IsRequired()
                     .HasMaxLength(255);

              builder.Property(u => u.Password)
                     .IsRequired()
                     .HasMaxLength(255);

              builder.Property(u => u.Role)
                     .IsRequired();

              builder.Property(u => u.ProfileId)
                     .IsRequired();

              builder.HasIndex(u => u.ProfileId)
                     .IsUnique();

              builder.HasOne<ProfileEntity>()
                     .WithOne()
                     .HasForeignKey<UserEntity>(u => u.ProfileId)
                     .OnDelete(DeleteBehavior.Cascade);
       }
}
