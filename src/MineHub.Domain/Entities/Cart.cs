using MineHub.Domain.Exceptions;
using MineHub.Domain.Shared;
using MineHub.Domain.ValueObjects;
using System.Text.Json.Serialization;

namespace MineHub.Domain.Entities;

public class Cart : AggregateRoot
{
    private List<CartItem> _cartItems = new();
    public Guid UserId { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime UpdatedAtUtc { get; private set; }
    public IReadOnlyCollection<CartItem> CartItems => _cartItems.AsReadOnly();

    public Cart(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new DomainException("UserId is required", "invalid_user_id");
        
        Id = Guid.NewGuid();
        UserId = userId;
        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = CreatedAtUtc;
    }

    public static Cart Rehydrate(
        Guid id,
        Guid userId,
        List<CartItem> items, 
        DateTime createdAt, 
        DateTime updatedAt)
    {
        var cart = new Cart(userId);
        cart.Id = id;
        cart._cartItems = items;
        cart.CreatedAtUtc = createdAt;
        cart.UpdatedAtUtc = updatedAt;

        return cart;
    }

    public void AddItem(Guid productId, int quantity)
    {
        if (productId == Guid.Empty)
            throw new DomainException("Product id is required", "product_id_is_required");
        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero", "invalid_quantity");

        var index = _cartItems.FindIndex(p => p.ProductId == productId);
        
        if (index != -1)
        {
            var existingItem = _cartItems[index];
            
            var updatedItem = new CartItem(
                productId, 
                existingItem.Quantity + quantity);

            _cartItems[index] = updatedItem;
            UpdatedAtUtc = DateTime.UtcNow;
            return;
        }

        _cartItems.Add(new CartItem(productId, quantity));
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void RemoveItem(Guid productId)
    {
        if (productId == Guid.Empty)
            throw new DomainException("Product is required", "invalid_product");

       var existingItem = _cartItems.FirstOrDefault(c => c.ProductId == productId);
       
        if (existingItem is null)
            throw new DomainException("Product was not found", "invalid_product_exist");

        _cartItems.Remove(existingItem);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Clear()
    {
        if (!_cartItems.Any())
            throw new DomainException("Cart is empty", "invalid_empty_cart");

        _cartItems.Clear();
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void ChangeQuantity(Guid productId, int quantity)
    {
        if (productId == Guid.Empty)
            throw new DomainException("Product Id is required", "invalid_product_id");
        
        if (quantity < 0)
            throw new DomainException("Quantity cannot be negative", "invalid_quantity");

        if (quantity == 0)
        {
            RemoveItem(productId);
            return;
        }

        var index = _cartItems.FindIndex(c => c.ProductId == productId);
        
        if (index == -1)
            throw new DomainException("Product is not found", "invalid_product_id");

        var updateItem = _cartItems[index].ChangeQuantity(quantity);
        _cartItems[index] = updateItem;

        UpdatedAtUtc = DateTime.UtcNow;
    }
}
