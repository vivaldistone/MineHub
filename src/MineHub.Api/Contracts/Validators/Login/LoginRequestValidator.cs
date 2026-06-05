using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;
using MineHub.Api.Contracts.Requests.Login;

namespace MineHub.Api.Contracts.Validators.Login;

public class LoginRequestValidator : AbstractValidator<LoginUserRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(r => r.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(r => r.Password)
            .NotEmpty();
    }
}
