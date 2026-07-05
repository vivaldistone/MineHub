namespace MineHub.Api.Contracts.Requests.ResetPassword;

public sealed record ResetPasswordRequest(string Email, string TokenReset, string NewPassword);
