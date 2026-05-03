using MineHub.Domain.Enums;
using MineHub.Domain.ValueObjects;
using MineHub.Domain.Exceptions;

namespace MineHub.Domain.Entities;

public class Order
{
    private readonly List<OrderItem> _orderItems = new();
    
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public OrderStatus Status { get; private set; }
    public decimal TotalPrice => _orderItems.Sum(o => o.TotalPrice);
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    private Order() { }
    
    private Order(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new DomainException("User id is required", "invalid_user_id");

        Id = Guid.NewGuid();
        UserId = userId;
        CreatedAtUtc = DateTime.UtcNow;
        Status = OrderStatus.Created;
    }

    public static Order Create(Cart cart)
    {
        if (cart is null)
            throw new DomainException("Cart is required", "invalid_cart");
        if (!cart.CartItems.Any())
            throw new DomainException("Cart is empty", "empty_cart");
        
        var order = new Order(cart.UserId);

        order._orderItems.AddRange(cart.CartItems.Select(c =>
        new OrderItem(c.ProductId,
        c.ProductName,
        c.Description,
        c.UnitPrice,
        c.Quantity)));

        return order;
    }

    public void Pay()
    {
        if (Status != OrderStatus.Created)
            throw new DomainException("Status not created", "invalid_order_status");    
            
        Status = OrderStatus.Paid;
    }

    public void Cancel()
    {
        if (Status != OrderStatus.Created)
            throw new DomainException("Status not created", "invalid_order_status");
        
        Status = OrderStatus.Cancelled;
    }
}
