using Microsoft.AspNetCore.Http;
using MineHub.Application.Abstractions.Persistence;
using MineHub.Application.Abstractions.Services;
using MineHub.Domain.Entities;

namespace MineHub.Infrastructure.Identity.Services;

internal class DomainUserService : ICurrentDomainUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;

    public DomainUserService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
    }

    public async Task<User> GetRequiredAsync()
    {
        var identityName = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;

        if (string.IsNullOrWhiteSpace(identityName))
            throw new UnauthorizedAccessException("user not authentication");

        var domainUser = await _userRepository.GetByIdentityUserIdAsync(identityName);

        if (domainUser is null)
            throw new UnauthorizedAccessException($"Domain user '{identityName}' was not found.");

        return domainUser;
    }
}
