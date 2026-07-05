using MineHub.Application.Abstractions.Auth;

namespace MineHub.Application.Auth.Commands.SendPasswordResetToken;

public class ForgotPasswordCommandHandler
{
    private readonly IAccountService _identityService;

    public ForgotPasswordCommandHandler(IAccountService identityService)
    {
        _identityService = identityService;
    }

    public async Task HandleAsync(string email, CancellationToken token)
    {
        await _identityService.RequestPasswordResetAsync(email, token);
    }
}
