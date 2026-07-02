using MineHub.Domain.Exceptions;
using System.Net.Mail;

namespace MineHub.Domain.ValueObjects;

public sealed record EmailAdress
{
    public string Value { get; }

    public EmailAdress(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Email is required", "email_is_required");

        value = value.Trim().ToLowerInvariant();

        if (!MailAddress.TryCreate(value, out _))
            throw new DomainException("Invalid email adress", "invalid_email_adress");

        Value = value;
    }

    public override string ToString()
    {
        return Value;
    }
}
