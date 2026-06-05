using FluentValidation;
using MineHub.Api.Contracts.Requests.AddItemToCart;

namespace MineHub.Api.Contracts.Validators.AddItemToCart;

public class AddItemToCartRequestValidator : AbstractValidator<AddItemToCartRequest>
{
    public AddItemToCartRequestValidator()
    {
        RuleFor(r => r.Id)
            .NotEmpty();

        RuleFor(r => r.Quantity)
            .GreaterThanOrEqualTo(1);
    }
}
