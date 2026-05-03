using MineHub.Domain.Exceptions;

namespace MineHub.Domain.ValueObjects;

public sealed record OrderItem
{
    public Guid ProductId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public decimal TotalPrice => UnitPrice * Quantity;

    public OrderItem(Guid productId, string name, string description, decimal unitPrice, int quantity)
    {
        if (productId == Guid.Empty)
            throw new DomainException("ProductId is required", "invalid_product_id");
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Name is required", "invalid_name");
        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Description is required", "invalid_description");
        if (unitPrice <= 0)
            throw new DomainException("Unit price must be greater than zero", "invalid_unit_price");
        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero", "invalid_quantity");

        ProductId = productId;
        Name = name.Trim();
        Description = description.Trim();
        UnitPrice = unitPrice;
        Quantity = quantity;
    }
}

