namespace MineHub.Application.Exceptions;

public class NotFoundException : Exception
{
    public string Code { get; private set; }
    public NotFoundException(string message, string code) : base(message)
    {
        Code = code;
    }
}
