using System.Security.Cryptography;
using MineHub.Application.Abstractions.Auth;

namespace MineHub.Infrastructure.Auth.Jwt;

public class RefreshTokenGenerator : IRefreshTokenGenerator
{
    public string Generate(int sizeInBytes = 64)
    {
        var bytes = RandomNumberGenerator.GetBytes(sizeInBytes);

        return Convert.ToBase64String(bytes);
    }
}
