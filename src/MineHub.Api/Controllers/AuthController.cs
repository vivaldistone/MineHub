using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MineHub.Api.Contracts.Requests.Login;
using MineHub.Api.Contracts.Requests.RefreshToken;
using MineHub.Api.Contracts.Requests.Register;
using MineHub.Application.Auth.Commands.Login;
using MineHub.Application.Auth.Commands.RefreshToken;
using MineHub.Application.Auth.Commands.Register;

namespace MineHub.Api.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly LoginUserCommandHandler _loginUserHandler;
    private readonly RegisterUserCommandHandler _registerUserHandler;
    private readonly RefreshTokenCommandHandler _refreshTokenHandler;

    public AuthController(LoginUserCommandHandler loginUserHandler, RegisterUserCommandHandler registerUserHandler, RefreshTokenCommandHandler refreshTokenHandler)
    {
        _loginUserHandler = loginUserHandler;
        _registerUserHandler = registerUserHandler;
        _refreshTokenHandler = refreshTokenHandler;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserRequest request, IValidator<RegisterUserRequest> validator)
    {
        var resultValidate = await validator.ValidateAsync(request);

        if (!resultValidate.IsValid)
            throw new ValidationException(resultValidate.Errors);
        
        var registerUserCommand = new RegisterUserCommand(request.Email, request.Password);

        await _registerUserHandler.HandleAsync(registerUserCommand);

        return NoContent();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserRequest request, IValidator<LoginUserRequest> validator)
    {
        var resultValidate = await validator.ValidateAsync(request);

        if (!resultValidate.IsValid)
            throw new ValidationException(resultValidate.Errors);

        var loginUserCommand = new LoginUserCommand(request.Email, request.Password);

        var result = await _loginUserHandler.HandleAsync(loginUserCommand);

        return Ok(result);
    }


    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenRequest request)
    {
        var refreshTokenCommand = new RefreshTokenCommand(request.Hash);

        var jwt = await _refreshTokenHandler.HandleAsync(refreshTokenCommand);

        return Ok(jwt);
    }
}

