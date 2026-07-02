using MineHub.Application.Carts.DTOs;
using MineHub.Domain.Entities;

namespace MineHub.Application.Abstractions.Carts;

public interface ICartService
{
    void AddItem(CartCacheDto cart, Product product, int quantity);
    void RemoveItem(CartCacheDto cart, Guid productId);
    void Clear(CartCacheDto cart);
    void ChangeQuantity(CartCacheDto cart, Guid productId, int quantity);
    bool RefreshItemsFromProducts(CartCacheDto cart, IEnumerable<Product> productsInCart);
}
