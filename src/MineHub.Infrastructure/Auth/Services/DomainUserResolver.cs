using Microsoft.AspNetCore.Http;
using MineHub.Application.Abstractions.Persistence;
using MineHub.Application.Abstractions.Users;
using MineHub.Domain.Entities;
using MineHub.Domain.ValueObjects;
using System.Security.Claims;

namespace MineHub.Infrastructure.Auth.Services;

internal class DomainUserResolver : IDomainUserResolver
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;

    public DomainUserResolver(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
    }

    public async Task<User> GetRequiredAsync(CancellationToken token)
    {
        var identityEmail = _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;

        if (string.IsNullOrWhiteSpace(identityEmail))
            throw new UnauthorizedAccessException("user not authentication");

        var emailAdress = new EmailAdress(identityEmail);

        var domainUser = await _userRepository.GetByIdentityUserEmailAsync(emailAdress, token);

        if (domainUser is null)
            throw new UnauthorizedAccessException($"Domain user '{identityEmail}' was not found.");

        return domainUser;
    }
}
