using MineHub.Domain.Entities;
namespace MineHub.Application.Abstractions.Services;

public interface ICurrentDomainUserService
{
    Task<User> GetRequiredAsync();
}
