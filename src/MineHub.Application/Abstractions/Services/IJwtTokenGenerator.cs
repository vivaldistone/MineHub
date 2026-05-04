namespace MineHub.Application.Abstractions.Services;

public interface IJwtTokenGenerator
{
    string GenerateToken(string userId, string email, IEnumerable<string> roles); 
}
