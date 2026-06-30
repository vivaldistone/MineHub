namespace MineHub.Application.Exceptions;

public abstract class ApplicationException : Exception
{
    public string Code { get; }

    protected ApplicationException(string message, string code) : base(message)
    {
        Code = code ?? throw new ArgumentNullException(nameof(code));
    }
}
