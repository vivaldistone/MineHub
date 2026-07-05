using Microsoft.Extensions.Options;
using MineHub.Application.Abstractions.Payments;
using MineHub.Application.Payments.Commands.CreatePayment;
using MineHub.Infrastructure.Payments.Services.YooKassaPayment.DTOs;
using MineHub.Infrastructure.Payments.Options;
using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace MineHub.Infrastructure.Payments.Services.YooKassaPayment;

internal class YooKassaPaymentProvider : IPaymentProvider
{
    private readonly HttpClient _httpClient;
    private readonly IOptions<YooKassaOptions> _options;

    public YooKassaPaymentProvider(HttpClient httpClient,
        IOptions<YooKassaOptions> options)
    {
        _httpClient = httpClient;
        _options = options;
    }

    public async Task<CreatePaymentProviderResult> CreatePaymentAsync(Guid orderId, Guid paymentId, decimal amount)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://api.yookassa.ru/v3/payments");

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Basic", _options.Value.AuthorizationToken);

        request.Headers.Add("Idempotence-Key", paymentId.ToString());

        var body = new YooKassaCreatePaymentRequest()
        {
            Amount = new Amount()
            {
                Value = amount.ToString("F2", CultureInfo.InvariantCulture),
                Currency = "RUB"
            },
            Capture = true,
            Confirmation = new Confirmation()
            {
                Type = "redirect",
                ReturnUrl = _options.Value.ReturnUrl
            },
            Description = $"Order : {orderId}"
        };

        request.Content = JsonContent.Create(body);
        
        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();

            throw new InvalidOperationException(
                 $"YooKassa payment creation failed. Status: {response.StatusCode}. Body: {error}");
        }

        var yooKassaResponse = await response.Content.ReadFromJsonAsync<YooKassaCreatePaymentResponse>();

        if (yooKassaResponse is null)
            throw new InvalidOperationException("YooKassa returned empty response");

        return new CreatePaymentProviderResult(
        yooKassaResponse.Id,
        yooKassaResponse.Confirmation.ConfirmationUrl);
    }
}
