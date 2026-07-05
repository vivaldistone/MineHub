using Microsoft.AspNetCore.Mvc;
using MineHub.Api.Contracts.Requests.Payment;
using MineHub.Application.Payments.Commands.CreatePayment;
using MineHub.Application.Payments.Commands.PaymentWebhook;
using System.Text.Json;

namespace MineHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly CreatePaymentCommandHandler _createPaymentHandler;
    private readonly PaymentWebhookCommandHandler _paymentWebhookHandler;

    public PaymentsController(CreatePaymentCommandHandler createPaymentHandler,
        PaymentWebhookCommandHandler paymentWebhookHandler)
    {
        _createPaymentHandler = createPaymentHandler;
        _paymentWebhookHandler = paymentWebhookHandler;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePayment(CancellationToken token)
    {
        var response = await _createPaymentHandler.HandleAsync(token);

        return Ok(response);
    }

    [HttpPost("webhook")]
    public async Task<IActionResult> WebHook()
    {
        using var reader = new StreamReader(Request.Body);
        var json = await reader.ReadToEndAsync();

        var request = JsonSerializer.Deserialize<YooKassaWebhookRequest>(json);

        return Ok(request);
    }

    [HttpGet]
    public async Task<IActionResult> Success()
    {
        return Ok("Оплата завершена");
    }
}
