namespace MineHub.Application.Exceptions;

public class ConflictException : Exception
{
    public string Code { get; private set; }

    public ConflictException(string message, string code) : base(message)
    {
        Code = code;
    }
}
