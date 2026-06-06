namespace MineHub.Application.Abstractions.Auth;

public interface IRefreshTokenGenerator
{
    string Generate(int sizeInBytes = 64);
}
