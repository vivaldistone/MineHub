using FluentValidation;
using MineHub.Api.Contracts.Requests.Products;

namespace MineHub.Api.Contracts.Validators.Products;

public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(30);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(100);

        RuleFor(x => x.Price)
            .GreaterThan(0);
    }
}
