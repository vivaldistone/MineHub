namespace MineHub.Application.Exceptions;

/// <summary>
/// Пользователь аутентифицирован, но ему нельзя выполнять действие
/// </summary>
public sealed class UnauthorizedException : ApplicationException
{
    public UnauthorizedException(string message, string code) 
        : base(message, code) 
    {
    }
}
