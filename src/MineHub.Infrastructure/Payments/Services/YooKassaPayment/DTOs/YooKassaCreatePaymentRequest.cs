using System.Text.Json.Serialization;

namespace MineHub.Infrastructure.Payments.Services.YooKassaPayment.DTOs;

public class YooKassaCreatePaymentRequest
{
    public Amount Amount { get; set; } = default!;
    public bool Capture { get; set; }
    public Confirmation Confirmation { get; set; } = default!;
    public string Description { get; set; } = string.Empty;

}

public class Amount
{
    public string Value { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
}

public class Confirmation
{
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("return_url")]
    public string ReturnUrl { get; set; } = string.Empty;
}