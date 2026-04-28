using MineHub.Domain.ValueObjects;
using MineHub.Domain.Exceptions;

namespace MineHub.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string AuthUserId { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public DateTime CreatedAtUtc { get; private set; }
    public MinecraftProfile? MinecraftProfile { get; private set; }

    private User() { }

    public User(string authUserId, string email)
    {
        if (string.IsNullOrWhiteSpace(authUserId))
            throw new DomainException("Auth User Id is required", "invalid_identity_user_id");
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("User Email is required", "invalid_user_email");

        Id = Guid.NewGuid();
        AuthUserId = authUserId.Trim();
        Email = email.Trim();
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void ChangeEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("User Email is required", "invalid_user_email");

        Email = email.Trim();
    }

    public void LinkMinecraftProfile(MinecraftProfile profile)
    {
        if (profile is null)
            throw new DomainException("Minecraft Profile is required", "invalid_minecraft_profile");

        MinecraftProfile = profile;
    }

   public void UnlinkMinecraftProfile()
   {
        if (MinecraftProfile is null)
            throw new DomainException("Minecraft Profile is not linked", "invalid_unlink_profile");
        
        MinecraftProfile = null;
   }
}
