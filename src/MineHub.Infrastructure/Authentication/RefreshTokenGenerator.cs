using System.Security.Cryptography;
using MineHub.Application.Abstractions.Services;

namespace MineHub.Infrastructure.Authentication;

public class CryptographicStringGenerator : ICryptographicStringGenerator
{
    public string Generate(int sizeInBytes = 64)
    {
        var bytes = RandomNumberGenerator.GetBytes(sizeInBytes);

        return Convert.ToBase64String(bytes);
    }
}
