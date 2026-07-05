namespace MineHub.Application.Exceptions;

public sealed class BusinessRuleException : ApplicationException
{
    public BusinessRuleException(string message, string code) 
        : base(message, code)
    {
    }
}
