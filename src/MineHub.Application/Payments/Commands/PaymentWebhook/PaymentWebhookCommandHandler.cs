using MineHub.Application.Abstractions.Persistence;
using MineHub.Application.Exceptions;

namespace MineHub.Application.Payments.Commands.PaymentWebhook;

public class PaymentWebhookCommandHandler
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PaymentWebhookCommandHandler(
        IPaymentRepository paymentRepository, 
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork)
    {
        _paymentRepository = paymentRepository;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(PaymentWebhookCommand command, CancellationToken token)
    {
        var payment = await _paymentRepository.GetByProviderPaymentIdAsync(command.ProviderPaymentId, token);

        if (payment is null)
        {
            throw new NotFoundException("Provider payment Id not found", "provider_payment_id_not_found");
        }

        var order = await _orderRepository.GetByIdAsync(payment.OrderId, token);

        if (order is null)
            throw new NotFoundException(
                "Order for payment must be not null",
                "order_for_payment_must_be_not_null");

        switch (command.Status)
        {
            case "pending":
                break;
            
            case "succeeded":
                payment.Succeed();
                order.MarkAsPaid(payment);
                break;
            
            case "canceled":
                payment.Cancel();
                break;

            default:
                throw new InvalidOperationException(
                    $"Unknown payment status: {command.Status}");
        }

        await _unitOfWork.SaveChangesAsync(token);
    }
}
