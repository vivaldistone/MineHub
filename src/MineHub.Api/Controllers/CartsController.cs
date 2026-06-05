using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MineHub.Api.Contracts.Requests.AddItemToCart;
using MineHub.Application.Carts.Commands.AddItemToCart;
using MineHub.Application.Carts.Queries.GetCart;

namespace MineHub.Api.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class CartsController : ControllerBase
{
    private readonly GetCartQueryHandler _getCartHandler;
    private readonly AddItemToCartCommandHandler _addItemToCartHandler;

    public CartsController(GetCartQueryHandler getCartHandler,
        AddItemToCartCommandHandler addItemToCartHandler)
    {
        _getCartHandler = getCartHandler;
        _addItemToCartHandler = addItemToCartHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        var cart = await _getCartHandler.HandleAsync();

        return Ok(cart);
    }

    [HttpPost]
    public async Task<IActionResult> AddItemToCart(AddItemToCartRequest request, IValidator<AddItemToCartRequest> validator)
    {
        var resultValidate = await validator.ValidateAsync(request);

        if (!resultValidate.IsValid)
            throw new ValidationException(resultValidate.Errors);
        
        var command = new AddItemToCartCommand(request.Id, request.Quantity);
        await _addItemToCartHandler.HandleAsync(command);

        return Created();
    }
}
