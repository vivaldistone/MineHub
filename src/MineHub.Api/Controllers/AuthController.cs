using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using MineHub.Api.Contracts.Requests.Login;
using MineHub.Api.Contracts.Requests.RefreshToken;
using MineHub.Api.Contracts.Requests.Register;
using ResetPasswordRequest = MineHub.Api.Contracts.Requests.ResetPassword.ResetPasswordRequest;
using MineHub.Application.Auth.Commands.Login;
using MineHub.Application.Auth.Commands.RefreshToken;
using MineHub.Application.Auth.Commands.Register;
using MineHub.Application.Auth.Commands.ResetPassword;
using MineHub.Application.Auth.Commands.SendPasswordResetToken;

namespace MineHub.Api.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly LoginUserCommandHandler _loginUserHandler;
    private readonly RegisterUserCommandHandler _registerUserHandler;
    private readonly RefreshTokenCommandHandler _refreshTokenHandler;
    private readonly ForgotPasswordCommandHandler _forgotPasswordHandler;
    private readonly ResetPasswordCommandHandler _resetPasswordHandler;

    public AuthController(LoginUserCommandHandler loginUserHandler, 
        RegisterUserCommandHandler registerUserHandler, 
        RefreshTokenCommandHandler refreshTokenHandler,
        ForgotPasswordCommandHandler forgotPasswordHandler,
        ResetPasswordCommandHandler resetPasswordHandler)
    {
        _loginUserHandler = loginUserHandler;
        _registerUserHandler = registerUserHandler;
        _refreshTokenHandler = refreshTokenHandler;
        _forgotPasswordHandler = forgotPasswordHandler;
        _resetPasswordHandler = resetPasswordHandler;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserRequest request, IValidator<RegisterUserRequest> validator, CancellationToken token)
    {
        var resultValidate = await validator.ValidateAsync(request);

        if (!resultValidate.IsValid)
            throw new ValidationException(resultValidate.Errors);
        
        var registerUserCommand = new RegisterUserCommand(request.Email, request.Password);

        await _registerUserHandler.HandleAsync(registerUserCommand, token);

        return NoContent();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserRequest request, IValidator<LoginUserRequest> validator, CancellationToken token)
    {
        var resultValidate = await validator.ValidateAsync(request);

        if (!resultValidate.IsValid)
            throw new ValidationException(resultValidate.Errors);

        var loginUserCommand = new LoginUserCommand(request.Email, request.Password);

        var result = await _loginUserHandler.HandleAsync(loginUserCommand, token);

        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenRequest request, CancellationToken token)
    {
        var refreshTokenCommand = new RefreshTokenCommand(request.Hash);

        var jwt = await _refreshTokenHandler.HandleAsync(refreshTokenCommand, token);

        return Ok(jwt);
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request, CancellationToken token)
    {
        await _forgotPasswordHandler.HandleAsync(request.Email, token);

        return Ok();
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordRequest request, CancellationToken token)
    {
        await _resetPasswordHandler.HandleAsync(
            request.Email, 
            request.TokenReset, 
            request.NewPassword, 
            token);

        return Ok();
    }

    //public async Task <IActionResult> ResetPassword()
    //{
    //    return Ok();
    //}

}

