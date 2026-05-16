namespace MineHub.Application.Carts.Queries;

public record GetCartResponse(Guid Id, DateTime CreatedAtUtc, DateTime UpdatedAtUtc, decimal TotalPrice, IReadOnlyCollection<GetCartItemResponse> CartItemsResponse);
