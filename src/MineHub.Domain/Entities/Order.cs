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
    public DateTime? PaidAtUtc { get; private set; }
    public DateTime? CancelledAtUtc { get; private set; }
    public decimal TotalPrice => _orderItems.Sum(o => o.TotalPrice);
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    private Order() { }
    
    public Order(Guid userId, IEnumerable<OrderItem> orderItems)
    {
        if (userId == Guid.Empty)
            throw new DomainException("User id must be not empty", "user_id_must_be_not_empty");

        if (orderItems is null)
            throw new DomainException("Order items are required", "order_items_are_required");

        var items = orderItems.ToList();

        if (!items.Any())
            throw new DomainException("Order items are empty", "order_items_empty");

        Id = Guid.NewGuid();
        UserId = userId;
        CreatedAtUtc = DateTime.UtcNow;
        Status = OrderStatus.Created;

        _orderItems.AddRange(items);
    }

    public void Pay()
    {
        if (Status != OrderStatus.Created)
            throw new DomainException("Status not created", "invalid_order_status");    
            
        Status = OrderStatus.Paid;
        PaidAtUtc = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (Status != OrderStatus.Created)
            throw new DomainException("Status not created", "invalid_order_status");
        
        Status = OrderStatus.Cancelled;
        CancelledAtUtc = DateTime.UtcNow;
    }
}
