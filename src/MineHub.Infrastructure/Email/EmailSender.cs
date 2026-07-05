using MineHub.Application.Abstractions.Email;
using MimeKit;
using MailKit.Net.Smtp;

namespace MineHub.Infrastructure.Email;

public class EmailSender : IEmailSender
{
    public async Task SendEmailAsync(string to, string subject, string htmlBody, CancellationToken token)
    {
        var message = new MimeMessage();

        message.From.Add(
            new MailboxAddress("MineHub project", "vanvest.1996@yandex.ru"));

        message.To.Add(
            new MailboxAddress("", to));

        message.Subject = subject;

        message.Body = new TextPart("html")
        {
            Text = htmlBody
        };

        using var client = new SmtpClient();

        await client.ConnectAsync(
            "smtp.yandex.ru", 465, true, token);

        await client.AuthenticateAsync(
            "vanvest.1996@yandex.ru", "zjslqxrfveeeqzvm", token);

        await client.SendAsync(message, token);

        await client.DisconnectAsync(true, token);
    }
}
