using MineHub.Application.Abstractions.Auth;
using MineHub.Application.Abstractions.Persistence;
using MineHub.Application.Exceptions;
using MineHub.Domain.Entities;
using MineHub.Domain.ValueObjects;

namespace MineHub.Application.Auth.Commands.Register;

public class RegisterUserCommandHandler
{
    private readonly IAccountService _identityService;
    private readonly IUserRepository _userRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(
        IAccountService identityService, 
        IUserRepository userRepository, 
        ICartRepository cartRepository,
        IUnitOfWork unitOfWork)
    {
        _identityService = identityService;
        _userRepository = userRepository;
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(RegisterUserCommand command, CancellationToken token)
    {
        var email = new EmailAdress(command.Email);
        
        var result = await _identityService.CreateUserAsync(email.Value, command.Password, token);

        if (!result.Success)
            throw new ConflictException("User already exists", "user_already_exists");

        var user = new User(result.UserId, email);
        await _userRepository.AddAsync(user, token);

        var cart = new Cart(user.Id);
        await _cartRepository.SaveAsync(cart, token);

        await _unitOfWork.SaveChangesAsync(token);
    }
}
