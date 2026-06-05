using FluentValidation;
using MineHub.Api.Contracts.Requests.Register;

namespace MineHub.Api.Contracts.Validators.Register;

public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserRequestValidator()
    {
        RuleFor(r => r.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(r => r.Password)
            .NotEmpty();
    }
}
