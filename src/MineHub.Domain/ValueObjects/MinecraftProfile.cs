using MineHub.Domain.Exceptions;

namespace MineHub.Domain.ValueObjects;

public sealed record MinecraftProfile
{
    public Guid MinecraftUuid { get;}
    public string NickName { get;} = string.Empty;

    public MinecraftProfile(Guid minecraftUuid, string nickName)
    {
        if (minecraftUuid == Guid.Empty)
            throw new DomainException("minecraftUuid is required", "invalid_minecraft_uuid");
        if (String.IsNullOrWhiteSpace(nickName))
            throw new DomainException("NickName is required", "invalid_nickname");

        MinecraftUuid = minecraftUuid;
        NickName = nickName.Trim();
    }
}
