using MineHub.Application.Payments.Commands.CreatePayment;

namespace MineHub.Application.Abstractions.Payments;

public interface IPaymentProvider
{
    Task<CreatePaymentProviderResult> CreatePaymentAsync(Guid orderId, Guid paymentId, decimal amount);
}
