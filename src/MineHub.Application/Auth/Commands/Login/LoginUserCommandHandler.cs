using MineHub.Application.Abstractions.Auth;
using MineHub.Application.Exceptions;

namespace MineHub.Application.Auth.Commands.Login;

public class LoginUserCommandHandler
{
    private readonly IIdentityService _identityService;

    public LoginUserCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<LoginUserResponse> HandleAsync(LoginUserCommand command)
    {
        if (command is null) 
            throw new ArgumentNullException(nameof(command));

        var result = await _identityService.LoginAsync(command.Email, command.Password);

        if (!result.Success)
            throw new InvalidCredentialsException("Invalid email or password", "email_or_password_invalid");

        return new LoginUserResponse(result.JwtToken, result.RefreshToken);
    }
}
