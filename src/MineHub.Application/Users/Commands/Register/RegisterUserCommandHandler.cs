using MineHub.Application.Abstractions.Services;
using MineHub.Application.Exceptions;

namespace MineHub.Application.Users.Commands.Register;

public class RegisterUserCommandHandler
{
    private readonly IIdentityService _identityService;

    public RegisterUserCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task HandleAsync(RegisterUserCommand command)
    {
        if (command is null) 
            throw new ArgumentNullException(nameof(command));

        var result = await _identityService.CreateUserAsync(command.Email, command.Password);

        if (!result.Success)
            throw new ConflictException("User name already exists", "user_already_exists");

        
    }
}
