using Microsoft.AspNetCore.Http;
using MineHub.Application.Abstractions.Auth;
using System.Security.Claims;

namespace MineHub.Infrastructure.Auth.Services;

public class CurrentIdentityContext : ICurrentIdentityContext
{
    private readonly IHttpContextAccessor _contextAccessor;

    public CurrentIdentityContext(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public string? UserId => _contextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
}
