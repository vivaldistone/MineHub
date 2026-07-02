using MineHub.Domain.Enums;
using MineHub.Domain.Exceptions;
using MineHub.Domain.Shared;

namespace MineHub.Domain.Entities;

public class Payment : AggregateRoot
{
    public Guid OrderId { get; private set; }
    public string? ProviderPaymentId { get; private set; }
    public decimal Amount { get; private set; }
    public PaymentStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? PaidAtUtc { get; private set; }

    private Payment() { }
    
    public Payment(Guid orderId, decimal amount)
    {
        if (orderId == Guid.Empty)
            throw new DomainException("Order id is required", "order_id_is_required");

        if (amount <= 0)
            throw new DomainException("Amount must be greater than zero", "amount_must_be_greater_than_zero");
        
        Id = Guid.NewGuid();
        OrderId = orderId;
        Amount = amount;
        Status = PaymentStatus.Pending;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void Succeed()
    {
        if (Status == PaymentStatus.Succeeded)
            return;

        if (Status != PaymentStatus.Pending)
            throw new DomainException("Payment already processed", "payment_already_processed");

        Status = PaymentStatus.Succeeded;
        PaidAtUtc = DateTime.UtcNow;
    }
    public void Cancel()
    {
        if (Status != PaymentStatus.Pending)
            throw new DomainException("Payment already processed", "payment_already_processed");

        Status = PaymentStatus.Cancelled;
    }

    public void SetProviderPaymentId(string providerPaymentId)
    {
        if (string.IsNullOrWhiteSpace(providerPaymentId))
            throw new DomainException(
            "Provider payment id is required",
            "provider_payment_id_is_required");

        ProviderPaymentId = providerPaymentId;
    }
}
