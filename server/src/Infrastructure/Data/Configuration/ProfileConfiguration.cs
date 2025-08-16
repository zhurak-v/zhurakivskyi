namespace Infrastructure.Data.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profile.Core.Entities;
using User.Core.Entities;

public class ProfileConfiguration : IEntityTypeConfiguration<ProfileEntity>
{
       public void Configure(EntityTypeBuilder<ProfileEntity> builder)
       {
              builder.HasKey(p => p.Id);

              builder.Property(p => p.NickName)
                     .HasMaxLength(50)
                     .IsRequired(false);

              builder.Property(p => p.FirstName)
                     .HasMaxLength(100)
                     .IsRequired(false);

              builder.Property(p => p.LastName)
                     .HasMaxLength(100)
                     .IsRequired(false);

              builder.Property(p => p.ProfilePicture)
                     .HasMaxLength(255)
                     .IsRequired(false);

              builder.Property(p => p.UserId)
                     .IsRequired();

              builder.HasIndex(p => p.UserId)
                     .IsUnique();

              builder.HasOne<UserEntity>()
                     .WithOne()
                     .HasForeignKey<ProfileEntity>(p => p.UserId)
                     .OnDelete(DeleteBehavior.Cascade);
       }
}
