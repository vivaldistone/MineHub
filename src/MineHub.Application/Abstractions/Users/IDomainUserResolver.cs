using MineHub.Domain.Entities;
namespace MineHub.Application.Abstractions.Users;

public interface IDomainUserResolver
{
    Task<User> GetRequiredAsync(CancellationToken token);
}
