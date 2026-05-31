namespace MineHub.Api.Contracts.Requests.Login;

public sealed record LoginUserRequest(string email, string password);
