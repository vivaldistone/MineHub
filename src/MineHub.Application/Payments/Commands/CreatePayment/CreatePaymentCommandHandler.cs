using MineHub.Application.Abstractions.Payments;
using MineHub.Application.Abstractions.Persistence;
using MineHub.Application.Abstractions.Users;
using MineHub.Application.Exceptions;
using MineHub.Domain.Entities;

namespace MineHub.Application.Payments.Commands.CreatePayment;

public class CreatePaymentCommandHandler
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentProvider _paymentProvider;
    private readonly IDomainUserResolver _currentDomainUserService;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePaymentCommandHandler(
        IOrderRepository orderRepository, 
        IPaymentProvider paymentProvider,
        IDomainUserResolver currentDomainUserService,
        IPaymentRepository paymentRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _paymentProvider = paymentProvider;
        _currentDomainUserService = currentDomainUserService;
        _paymentRepository = paymentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreatePaymentProviderResult> HandleAsync(CancellationToken token)
    {
        var user = await  _currentDomainUserService.GetRequiredAsync(token);

        var createdOrder = await _orderRepository.GetCreatedByUserIdAsync(user.Id, token);

        if (createdOrder is null)
            throw new NotFoundException("Not order in created status", "not_order_in_created_status");

        var payment = new Payment(createdOrder.Id, createdOrder.TotalPrice);
        
        await _paymentRepository.AddAsync(payment, token);

        var result = await _paymentProvider.CreatePaymentAsync(
            createdOrder.Id, 
            payment.Id, 
            createdOrder.TotalPrice);

        payment.SetProviderPaymentId(result.ProviderPaymentId);

        await _unitOfWork.SaveChangesAsync(token);

        return result;
    }
}
