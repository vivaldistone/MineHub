using MineHub.Application.Abstractions.Persistence;
using MineHub.Application.Abstractions.Users;
using MineHub.Application.Exceptions;
using MineHub.Domain.Entities;
using MineHub.Domain.ValueObjects;

namespace MineHub.Application.Orders.Commands.CreateOrderFromCart;

public class CreateOrderFromCartCommandHandler
{
    private readonly IOrderRepository _orderRepository;
    private readonly IDomainUserResolver _domainUserResolver;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;

    public CreateOrderFromCartCommandHandler(IOrderRepository orderRepository, 
        IDomainUserResolver domainUserResolver,
        ICartRepository cartRepository,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _domainUserResolver = domainUserResolver;
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> HandleAsync(CancellationToken token)
    {
        var domainUser = await _domainUserResolver.GetRequiredAsync(token);

        var createOrder = await _orderRepository.GetCreatedByUserIdAsync(domainUser.Id, token);

        if (createOrder is not null)
        {
            createOrder.Cancel();
        }

        var cart = await _cartRepository.GetByUserIdAsync(domainUser.Id, token);
        
        if (cart is null)
        {
            throw new NotFoundException("user cart not found", "user_cart_not_found");
        }

        var productsIds = cart.CartItems.Select(c => c.ProductId).ToList();

        var products = await _productRepository.GetByIdsAsync(productsIds, token);

        foreach (var product in products)
        {
            if (!product.IsActive)
                throw new BusinessRuleException("Product in cart is not active", "product_in_cart_is_not_active");
        }

        var productsInCartRow = products.Join(
            cart.CartItems,
            p => p.Id,
            c => c.ProductId,
            (p, c) => new
            {
                ProductId = p.Id,
                Name = p.Name,
                Description = p.Description,
                UnitPrice = p.Price,
                Quantity = c.Quantity,
                TotalPrice = p.Price * c.Quantity
            }).ToList();

        var order = new Order(domainUser.Id,
            productsInCartRow.Select(p =>
                new OrderItem(
                    p.ProductId,
                    p.Name,
                    p.Description,
                    p.UnitPrice,
                    p.Quantity))
            .ToList());

        cart.Clear();

        await _orderRepository.AddAsync(order, token);

        await _unitOfWork.SaveChangesAsync(token);

        return order.Id;
    }
}
