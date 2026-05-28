using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MineHub.Domain.Entities;

namespace MineHub.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_tokens");

        builder.HasKey(token => token.Id);

        builder.Property(token => token.UserId)
            .HasColumnName("user_id");

        builder.HasOne<User>()
            .WithOne()
            .HasForeignKey<RefreshToken>(u => u.UserId)
            .HasPrincipalKey<User>(u => u.AuthUserId);

        builder.HasIndex(token => token.UserId)
            .IsUnique();

        builder.Property(token => token.HashToken)
            .HasColumnName("token_hash");

        builder.HasIndex(token => token.HashToken)
            .IsUnique();

        builder.Property(token => token.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();

        builder.Property(token => token.ExpiredAtUtc)
            .HasColumnName("expired_at_utc")
            .IsRequired();

        builder.Property(token => token.RevokedAtUtc)
            .HasColumnName("revoked_at_utc");

        builder.Ignore(token => token.IsRevoked);

        builder.Ignore(token => token.IsExpired);

        builder.Ignore(token => token.IsActive);
    }
}
