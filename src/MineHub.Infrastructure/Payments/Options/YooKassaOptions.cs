using System.Text;

namespace MineHub.Infrastructure.Payments.Options;

public class YooKassaOptions
{
    public string ShopId { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string ReturnUrl { get; set; } = string.Empty;

    public string AuthorizationToken => Convert.ToBase64String(Encoding.UTF8.GetBytes($"{ShopId}:{SecretKey}"));
}
