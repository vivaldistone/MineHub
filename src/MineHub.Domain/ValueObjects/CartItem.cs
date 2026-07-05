using MineHub.Domain.Exceptions;

namespace MineHub.Domain.ValueObjects;

public sealed record CartItem
{
    public Guid ProductId { get;}
    public int Quantity { get;}

    public CartItem(Guid productId, int quantity)
    {
        if (productId == Guid.Empty)
            throw new DomainException("Product Id is required", "invalid_product_id");
        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero", "invalid_quantity");
        
        ProductId = productId;
        Quantity = quantity;
    }

    public CartItem ChangeQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero", "invalid_quantity");
        
        return new CartItem(ProductId, quantity);
    }
}
