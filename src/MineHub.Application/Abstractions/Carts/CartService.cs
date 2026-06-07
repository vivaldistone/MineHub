using MineHub.Application.Abstractions.Cache.DTOs;
using MineHub.Domain.Entities;
using MineHub.Domain.Exceptions;

namespace MineHub.Application.Abstractions.Carts;

public class CartService : ICartService
{
    public void AddItem(CartCacheDto cart, Product product, int quantity)
    {
        if (product is null)
            throw new DomainException("Product is required", "invalid_product");
        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero", "invalid_quantity");
        if (!product.IsActive)
            throw new DomainException("Product is not active", "product_not_active");

        var index = cart.CartItems.FindIndex(p => p.ProductId == product.ProductId);

        if (index != -1)
        {
            var existingItem = cart.CartItems[index];

            var updatedItem = new CartItemCacheDto
            (
                product.ProductId,
                product.Name,
                product.Description,
                product.Price,
                quantity
            );

            cart.CartItems[index] = updatedItem;
            cart.UpdatedAtUtc = DateTime.UtcNow;
            return;
        }

        cart.CartItems.Add(new CartItemCacheDto(product.ProductId, product.Name, product.Description, product.Price, quantity));
        cart.UpdatedAtUtc = DateTime.UtcNow;
    }

    public void ChangeQuantity(CartCacheDto cart, Guid productId, int quantity)
    {
        if (productId == Guid.Empty)
            throw new DomainException("Product Id is required", "invalid_product_id");

        if (quantity < 0)
            throw new DomainException("Quantity cannot be negative", "invalid_quantity");

        if (quantity == 0)
        {
            RemoveItem(cart,productId);
            return;
        }

        var index = cart.CartItems.FindIndex(c => c.ProductId == productId);

        if (index == -1)
            throw new DomainException("Product is not found", "invalid_product_id");

        cart.CartItems[index].Quantity = quantity;

        cart.UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Clear(CartCacheDto cart)
    {
        if (!cart.CartItems.Any())
            throw new DomainException("Cart is empty", "invalid_empty_cart");

        cart.CartItems.Clear();
        cart.UpdatedAtUtc = DateTime.UtcNow;
    }

    public void RefreshItemsFromProducts(CartCacheDto cart, IEnumerable<Product> productsInCart)
    {
        var wasUpdatedCart = false;

        var productsById = productsInCart.ToDictionary(p => p.ProductId);

        for (int i = 0; i < productsById.Count; i++)
        {
            var cartItem = cart.CartItems[i];

            if (!productsById.TryGetValue(cartItem.ProductId, out var product))
                continue;

            if (cartItem.ProductName == product.Name &&
                cartItem.Description == product.Description &&
                cartItem.UnitPrice == product.Price)
                continue;

            cart.CartItems[i] = new CartItemCacheDto(
            product.ProductId,
            product.Name,
            product.Description,
            product.Price,
            cartItem.Quantity);

            wasUpdatedCart = true;
        }

        if (wasUpdatedCart)
            cart.UpdatedAtUtc = DateTime.UtcNow;
    }

    public void RemoveItem(CartCacheDto cart, Guid productId)
    {
        if (productId == Guid.Empty)
            throw new DomainException("Product is required", "invalid_product");

        var existingItem = cart.CartItems.FirstOrDefault(c => c.ProductId == productId);

        if (existingItem is null)
            throw new DomainException("Product was not found", "invalid_product_exist");

        cart.CartItems.Remove(existingItem);
        cart.UpdatedAtUtc = DateTime.UtcNow;
    }
}
