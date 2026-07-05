using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MineHub.Application.Orders.Commands.CancelOrder;
using MineHub.Application.Orders.Commands.CreateOrderFromCart;
using MineHub.Application.Orders.Queries.GetOrder;
using MineHub.Application.Orders.Queries.GetOrders;
using MineHub.Application.Orders.Queries.GetUserOrder;
using MineHub.Application.Orders.Queries.GetUserOrdersById;

namespace MineHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly CreateOrderFromCartCommandHandler _createOrderFromCartHandler;
    private readonly GetUserOrderQueryHandler _getUserOrderHandler;
    private readonly GetUserOrdersQueryHandler _getUserOrdersHandler;

    public OrdersController(CancelOrderCommandHandler cancelOrderHandler, 
        CreateOrderFromCartCommandHandler createOrderFromCartHandler,
        GetUserOrderQueryHandler getUserOrderHandler,
        GetUserOrdersQueryHandler getUserOrdersHandler,
        GetOrdersQueryHandler getOrdersHandler,
        GetOrderQueryHandler getOrderHandler,
        GetUserOrdersByIdQueryHandler getUserOrdersByIdHandler)
    {
        _createOrderFromCartHandler = createOrderFromCartHandler;
        _getUserOrderHandler = getUserOrderHandler;
        _getUserOrdersHandler = getUserOrdersHandler;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetMyOrders(CancellationToken token)
    {
        var result = await _getUserOrdersHandler.HandleAsync(token);

        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetMyOrderById(Guid id, CancellationToken token)
    {
        var result = await _getUserOrderHandler.HandleAsync(id, token);

        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateOrder(CancellationToken token)
    {
        var orderId = await _createOrderFromCartHandler.HandleAsync(token);

        return CreatedAtAction(nameof(GetMyOrderById),
            new { id = orderId },
            new { id = orderId });
    }
}
