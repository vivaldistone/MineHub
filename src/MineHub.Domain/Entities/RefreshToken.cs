using MineHub.Domain.Exceptions;

namespace MineHub.Domain.Entities;

public sealed class RefreshToken
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string HashToken { get; private set; } = String.Empty;
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime ExpiredAtUtc { get; private set; }
    public DateTime? RevokedAtUtc { get; private set; }
    public bool IsRevoked => RevokedAtUtc is not null;
    public bool IsExpired => DateTime.Now >= ExpiredAtUtc;
    public bool IsActive => !IsRevoked && !IsExpired;

    private RefreshToken() { }

    public RefreshToken(Guid userId, string tokenHash, DateTime expiredAtUtc)
    {
        if (userId == Guid.Empty)
            throw new DomainException("User Id is required", "invalid_user_id");
        if (string.IsNullOrWhiteSpace(tokenHash))
            throw new DomainException("Token hash is required", "invalid_refresh_token");

        Id = Guid.NewGuid();
        UserId = userId;
        HashToken = tokenHash;
        CreatedAtUtc = DateTime.UtcNow;
        ExpiredAtUtc = expiredAtUtc;
    }

    public void Revoke()
    {
        if (IsRevoked)
            return;

        RevokedAtUtc = DateTime.UtcNow;
    }


}
