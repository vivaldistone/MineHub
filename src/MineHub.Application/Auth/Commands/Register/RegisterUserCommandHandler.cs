using MineHub.Application.Abstractions.Auth;
using MineHub.Application.Abstractions.Persistence;
using MineHub.Application.Exceptions;
using MineHub.Domain.Entities;

namespace MineHub.Application.Auth.Commands.Register;

public class RegisterUserCommandHandler
{
    private readonly IIdentityService _identityService;
    private readonly IUserRepository _userRepository;
    private readonly ICartRepository _cartRepository;


    public RegisterUserCommandHandler(IIdentityService identityService, IUserRepository userRepository, ICartRepository cartRepository)
    {
        _identityService = identityService;
        _userRepository = userRepository;
        _cartRepository = cartRepository;
    }

    public async Task HandleAsync(RegisterUserCommand command)
    {
        var result = await _identityService.CreateUserAsync(command.Email, command.Password);

        if (!result.Success)
            throw new ConflictException("User already exists", "user_already_exists");

        var user = new User(result.UserId, command.Email);
        await _userRepository.AddAsync(user);

        var cart = new Cart(user.Id);
        await _cartRepository.AddAsync(cart); 
    }
}
