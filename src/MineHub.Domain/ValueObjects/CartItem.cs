using MineHub.Domain.Exceptions;

namespace MineHub.Domain.ValueObjects;

public sealed record CartItem
{
    public Guid ProductId { get;}
    public string ProductName { get;}
    public string Description { get;}
    public decimal UnitPrice { get;}
    public int Quantity { get;}
    public decimal TotalPrice => UnitPrice * Quantity;

    public CartItem(Guid productId, string productName, string description, decimal unitPrice, int quantity)
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

    public CartItem ChangeQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero", "invalid_quantity");
        
        return new CartItem(ProductId, ProductName, Description, UnitPrice, quantity);
    }
}
