using System.Text.Json.Serialization;

namespace MineHub.Api.Contracts.Requests.Payment;

public sealed record YooKassaWebhookRequest
{
    public string Type { get; init; } = string.Empty;
    public string Event { get; init; } = string.Empty;

    [JsonPropertyName("object")]
    public YooKassaPayment Payment { get; init; } = default!;
}

public sealed record YooKassaPayment
{
    public string Id { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public bool Paid { get; init; }
    [JsonPropertyName("amount")]
    public YooKassaAmount Amount { get; init; } = default!;
}


public sealed record YooKassaAmount
{
    public string Value { get; init; } = string.Empty;
    public string Currency { get; init; } = string.Empty;
}
