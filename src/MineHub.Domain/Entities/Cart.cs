using MineHub.Domain.ValueObjects;
using MineHub.Domain.Exceptions;

namespace MineHub.Domain.Entities;

public class Cart
{
    private List<CartItem> _cartItems = new (); 
    
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime UpdatedAtUtc { get; private set; }
    public IReadOnlyCollection<CartItem> CartItems => _cartItems.AsReadOnly();
    public decimal TotalPrice => _cartItems.Sum(c => c.TotalPrice);

    public Cart(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new DomainException("UserId is required", "invalid_user_id");
        
        Id = Guid.NewGuid();
        UserId = userId;
        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = CreatedAtUtc;
    }

    public void AddItem(Product product, int quantity)
    {
        if (product is null)
            throw new DomainException("Product is required", "invalid_product");
        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero", "invalid_quantity");
        if (!product.IsActive)
            throw new DomainException("Product is not active", "product_not_active");

        var index = _cartItems.FindIndex(p => p.ProductId == product.ProductId);
        
        if (index != -1)
        {
            var existingItem = _cartItems[index];
            
            var updatedItem = new CartItem(
                product.ProductId, 
                product.Name, 
                product.Price, 
                existingItem.Quantity + quantity);

            _cartItems[index] = updatedItem;
            UpdatedAtUtc = DateTime.UtcNow;
            return;
        }

        _cartItems.Add(new CartItem(product.ProductId, product.Name, product.Price, quantity));
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
        
        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero", "invalid_quantity");

        var index = _cartItems.FindIndex(c => c.ProductId == productId);
        
        if (index == -1)
            throw new DomainException("Product is not found", "invalid_product_id");

        var updateItem = _cartItems[index].ChangeQuantity(quantity);
        _cartItems[index] = updateItem;

        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void RefreshItemsFromProducts(IEnumerable<Product> productsInCart)
    {
        var wasUpdatedCart = false;
        
        foreach (var product in productsInCart)
        {
            var index = _cartItems.FindIndex(c => c.ProductId == product.ProductId);
            
            if (index == -1)
                continue;
            
            var cartItem = _cartItems[index];
            
            if (cartItem.ProductName != product.Name || cartItem.UnitPrice != product.Price)
            {
                var updatedCartItem = new CartItem(cartItem.ProductId, product.Name, product.Price, cartItem.Quantity);

                _cartItems[index] = updatedCartItem;

                wasUpdatedCart = true;
            }
        }

        if (wasUpdatedCart)
            UpdatedAtUtc =DateTime.UtcNow;
    }

}
