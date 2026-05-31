namespace MineHub.Api.Contracts.Requests.AddItemToCart;

public sealed record AddItemToCartRequest(Guid Id, int Quantity);