namespace MineHub.Application.Exceptions;
/// <summary>
/// Учётные данные пользователя неверны.
/// </summary>
public sealed class InvalidCredentialsException : ApplicationException
{
    public InvalidCredentialsException(string message, string code) 
        : base (message, code)
    {
    }
}
