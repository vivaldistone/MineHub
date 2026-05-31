using Microsoft.AspNetCore.Mvc;
using MineHub.Api.Contracts.Requests.Login;
using MineHub.Api.Contracts.Requests.Register;
using MineHub.Application.Auth.Commands.Login;
using MineHub.Application.Auth.Commands.Register;

namespace MineHub.Api.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class AuthController : ControllerBase
{
    private LoginUserCommandHandler _loginUserHandler;
    private RegisterUserCommandHandler _registerUserHandler;

    public AuthController(LoginUserCommandHandler loginUserHandler, RegisterUserCommandHandler registerUserHandler)
    {
        _loginUserHandler = loginUserHandler;
        _registerUserHandler = registerUserHandler;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserRequest request)
    {
        var registerUserCommand = new RegisterUserCommand(request.email, request.password);

        await _registerUserHandler.HandleAsync(registerUserCommand);

        return Created();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserRequest request)
    {
        var loginUserCommand = new LoginUserCommand(request.email, request.password);

        var result = await _loginUserHandler.HandleAsync(loginUserCommand);

        return Ok(result);
    }
}

