using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MineHub.Application.Orders.Queries.GetOrder;
using MineHub.Application.Orders.Queries.GetOrders;
using MineHub.Application.Orders.Queries.GetUserOrdersById;

namespace MineHub.Api.Controllers;

[ApiController]
[Route("admin")]
[Authorize(Roles = "Admin")]
public class AdminOrdersController : ControllerBase
{
    private readonly GetOrderQueryHandler _getOrderHandler;
    private readonly GetOrdersQueryHandler _getOrdersHandler;
    private readonly GetUserOrdersByIdQueryHandler _getUserOrdersByIdHandler;

    public AdminOrdersController(
        GetOrderQueryHandler getOrderHandler,
        GetOrdersQueryHandler getOrdersHandler,
        GetUserOrdersByIdQueryHandler getUserOrdersHandler)
    {
        _getOrderHandler = getOrderHandler;
        _getOrdersHandler = getOrdersHandler;
        _getUserOrdersByIdHandler = getUserOrdersHandler;
    }


    [HttpGet("order/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetOrder(Guid id, CancellationToken token)
    {
        var result = await _getOrderHandler.HandleAsync(id, token);

        return Ok(result);
    }

    [HttpGet("orders")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetOrders(CancellationToken token)
    {
        var result = await _getOrdersHandler.HandleAsync(token);

        return Ok(result);
    }

    [HttpGet("user/{userId}/orders")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetOrdersByUserId(Guid userId, CancellationToken token)
    {
        var result = await _getUserOrdersByIdHandler.HandleAsync(userId, token);

        return Ok(result);
    }
}
