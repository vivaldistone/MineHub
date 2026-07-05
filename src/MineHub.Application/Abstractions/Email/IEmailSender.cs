namespace MineHub.Application.Abstractions.Email;

public interface IEmailSender
{
    Task SendEmailAsync(string to, string subject, string htmlBody, CancellationToken token);
}
