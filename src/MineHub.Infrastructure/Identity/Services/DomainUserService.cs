using Microsoft.AspNetCore.Http;
using MineHub.Application.Abstractions.Persistence;
using MineHub.Application.Abstractions.Services;
using MineHub.Domain.Entities;
using System.Security.Claims;

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
        var identityEmail = _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;

        if (string.IsNullOrWhiteSpace(identityEmail))
            throw new UnauthorizedAccessException("user not authentication");

        var domainUser = await _userRepository.GetByIdentityUserEmailAsync(identityEmail);

        if (domainUser is null)
            throw new UnauthorizedAccessException($"Domain user '{identityEmail}' was not found.");

        return domainUser;
    }
}
