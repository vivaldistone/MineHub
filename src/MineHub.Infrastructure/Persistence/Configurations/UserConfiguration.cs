using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MineHub.Domain.Entities;
using MineHub.Infrastructure.Identity;

namespace MineHub.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        
        builder.HasKey(u => u.Id);

        builder.Property(u => u.AuthUserId)
            .HasColumnName("auth_user_id")
            .HasMaxLength(255)
            .IsRequired();

        builder.HasIndex(u => u.AuthUserId)
            .IsUnique();

        builder.HasOne<AuthUser>()
            .WithOne()
            .HasForeignKey<User>(c => c.AuthUserId);

        builder.Property(u => u.Email)
            .HasColumnName("email")
            .HasMaxLength(255)
            .IsRequired();

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();

        builder.OwnsOne(u => u.MinecraftProfile, profile =>
        {
            profile.Property(p => p.MinecraftUuid)
                .HasColumnName("minecraft_uuid");

            profile.Property(p => p.NickName)
                .HasColumnName("minecraft_nickname")
                .HasMaxLength(255);
        });
    }
}
