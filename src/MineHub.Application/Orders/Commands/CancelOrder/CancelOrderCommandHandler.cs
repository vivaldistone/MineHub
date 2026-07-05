using MineHub.Application.Abstractions.Persistence;
using MineHub.Application.Abstractions.Users;
using MineHub.Application.Exceptions;

namespace MineHub.Application.Orders.Commands.CancelOrder;

public class CancelOrderCommandHandler
{
    private readonly IDomainUserResolver _currentDomainUserService;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelOrderCommandHandler(
        IDomainUserResolver currentDomainUserService, 
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork)
    {
        _currentDomainUserService = currentDomainUserService;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(CancellationToken token)
    {
        var user = await _currentDomainUserService.GetRequiredAsync(token);

        var order = await _orderRepository.GetCreatedByUserIdAsync(user.Id, token);

        if (order is null)
            throw new NotFoundException("Created order not found", "created_order_not_found");

        order.Cancel();

        await _unitOfWork.SaveChangesAsync(token);
    }
}
