namespace MineHub.Application.Payments.Commands.PaymentWebhook;

public sealed record PaymentWebhookCommand(
    string Event,
    string ProviderPaymentId,
    string Status,
    bool Paid);
