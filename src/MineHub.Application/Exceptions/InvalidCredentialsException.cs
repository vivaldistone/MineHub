namespace MineHub.Application.Exceptions;

public class InvalidCredentialsException : Exception
{
    public string StatusCode { get; private set; }
    public InvalidCredentialsException(string message, string statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}
