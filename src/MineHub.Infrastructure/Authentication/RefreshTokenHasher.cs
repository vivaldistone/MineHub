using MineHub.Application.Abstractions.Services;
using System.Security.Cryptography;
using System.Text;

namespace MineHub.Infrastructure.Authentication;

public class RefreshTokenHasher : IRefreshTokenHasher
{
    public string Hash(string refreshToken)
    {       
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken));

        return Convert.ToBase64String(bytes);
    }
}
