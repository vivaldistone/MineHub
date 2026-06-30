namespace MineHub.Application.Exceptions;

public sealed class ConflictException : ApplicationException
{
    public ConflictException(string message, string code) 
        : base(message, code)
    { 
    }
}
