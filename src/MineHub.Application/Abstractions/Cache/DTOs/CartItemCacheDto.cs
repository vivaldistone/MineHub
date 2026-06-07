using MineHub.Domain.Exceptions;

namespace MineHub.Application.Abstractions.Cache.DTOs;

public class CartItemCacheDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice => UnitPrice * Quantity;

    public CartItemCacheDto(Guid productId, string productName, string description, decimal unitPrice, int quantity)
    {
        if (productId == Guid.Empty)
            throw new DomainException("Product Id is required", "invalid_product_id");
        if (string.IsNullOrWhiteSpace(productName))
            throw new DomainException("Product Name is required", "invalid_product_name");
        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Description is required", "invalid_description");
        if (unitPrice <= 0)
            throw new DomainException("UnitPrice must be greater than zero", "invalid_unit_price");
        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero", "invalid_quantity");

        ProductId = productId;
        ProductName = productName.Trim();
        Description = description.Trim();
        UnitPrice = unitPrice;
        Quantity = quantity;
    }
}
