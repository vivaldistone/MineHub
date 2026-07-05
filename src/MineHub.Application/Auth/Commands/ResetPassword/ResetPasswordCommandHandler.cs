using MineHub.Application.Abstractions.Auth;

namespace MineHub.Application.Auth.Commands.ResetPassword;

public class ResetPasswordCommandHandler
{
    private readonly IAccountService _identityService;

    public ResetPasswordCommandHandler(IAccountService identityService)
    {
        _identityService = identityService;
    }

    public async Task HandleAsync(string email, string resetToken, string newPassword, CancellationToken token)
    {
        await _identityService.ResetPasswordAsync(email, resetToken, newPassword, token);
    }
}
