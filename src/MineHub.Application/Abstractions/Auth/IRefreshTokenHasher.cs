namespace MineHub.Application.Abstractions.Auth;

public interface IRefreshTokenHasher
{
    string Hash(string refreshToken);
}
