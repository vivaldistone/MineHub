namespace MineHub.Application.Abstractions.Services;

public interface IRefreshTokenHasher
{
    string Hash(string refreshToken);
}
