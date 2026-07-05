using MineHub.Application.Abstractions.Auth;
using System.Security.Cryptography;
using System.Text;

namespace MineHub.Infrastructure.Auth.Hashing;

public class RefreshTokenHasher : IRefreshTokenHasher
{
    public string Hash(string refreshToken)
    {       
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken));

        return Convert.ToBase64String(bytes);
    }
}
