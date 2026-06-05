using FluentValidation;
using MineHub.Api.Contracts.Requests.Products;

namespace MineHub.Api.Contracts.Validators.Products;

public class UpdateProductRequestValidatior : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductRequestValidatior()
    {
        RuleFor(r => r.Name)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(30);

        RuleFor(r => r.Description)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(100);

        RuleFor(r => r.Price)
            .GreaterThan(0);
    }
}
