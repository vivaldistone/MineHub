using MineHub.Domain.ValueObjects;
using MineHub.Domain.Exceptions;

namespace MineHub.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string AuthUserId { get; private set; } = string.Empty;
    public Email Email { get; private set; } 
    public DateTime CreatedAtUtc { get; private set; }
    public MinecraftProfile? MinecraftProfile { get; private set; }

    private User() { }

    public User(string authUserId, Email email)
    {
        if (string.IsNullOrWhiteSpace(authUserId))
            throw new DomainException("Auth User Id is required", "invalid_identity_user_id");

        Id = Guid.NewGuid();
        AuthUserId = authUserId.Trim();
        Email = email;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void ChangeEmail(Email email)
    {
        Email = email;
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
