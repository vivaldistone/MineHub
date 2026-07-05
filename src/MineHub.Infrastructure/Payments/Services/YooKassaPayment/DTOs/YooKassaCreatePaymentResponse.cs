using System.Text.Json.Serialization;

namespace MineHub.Infrastructure.Payments.Services.YooKassaPayment.DTOs;

public class YooKassaCreatePaymentResponse
{
    public string Id { get; set; }

    public YooKassaConfirmationResponse Confirmation { get; set; } = default!;
}

public class YooKassaConfirmationResponse
{
    public string Type { get; set; }
    [JsonPropertyName("confirmation_url")]
    public string ConfirmationUrl { get; set; }
}