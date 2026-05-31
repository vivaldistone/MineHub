namespace MineHub.Application.Exceptions;

public class UnauthorizedException : Exception
{
    public string Code { get; private set; }
    public UnauthorizedException(string message, string code) : base(message)
    {
        Code = code;
    }
}
