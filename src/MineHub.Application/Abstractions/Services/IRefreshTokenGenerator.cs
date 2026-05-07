namespace MineHub.Application.Abstractions.Services;

public interface IRefreshTokenGenerator
{
    string Generate(int sizeInBytes = 64);
}
