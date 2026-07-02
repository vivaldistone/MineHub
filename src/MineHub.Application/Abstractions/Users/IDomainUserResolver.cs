using MineHub.Domain.Entities;
namespace MineHub.Application.Abstractions.Users;

public interface ICurrentDomainUserService
{
    Task<User> GetRequiredAsync(CancellationToken token);
}
