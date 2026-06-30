namespace MineHub.Application.Exceptions;

public sealed class NotFoundException : ApplicationException
{
    public NotFoundException(string message, string code) 
        : base(message, code)
    {
    }
}
