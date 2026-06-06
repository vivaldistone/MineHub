namespace MineHub.Application.Abstractions.Auth;

public interface IJwtTokenGenerator
{
    string GenerateToken(string userId, string email, IEnumerable<string> roles); 
}
