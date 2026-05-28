using Microsoft.Extensions.DependencyInjection;
using MineHub.Application.Carts.Commands.AddItemToCart;
using MineHub.Application.Carts.Commands.ChangeCartItemQuantity;
using MineHub.Application.Carts.Queries.GetCart;
using MineHub.Application.Products.Commands.AddProduct;
using MineHub.Application.Products.Commands.DeleteProduct;
using MineHub.Application.Products.Commands.UpdateProduct;
using MineHub.Application.Products.Queries.GetProduct;
using MineHub.Application.Products.Queries.GetProducts;
using MineHub.Application.Users.Queries.GetUser;
using MineHub.Application.Users.Queries.GetUsers;

namespace MineHub.Application.DependencyInjection;

public static class DependencyInjection 
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<GetProductsQueryHandler>();
        services.AddScoped<GetProductQueryHandler>();
        services.AddScoped<AddProductCommandHandler>();
        services.AddScoped<DeleteProductCommandHandler>();
        services.AddScoped<UpdateProductCommandHandler>();

        services.AddScoped<GetCartQueryHandler>();
        services.AddScoped<AddItemToCartCommandHandler>();
        services.AddScoped<ChangeCartItemQuantityCommandHandler>();

        services.AddScoped<GetUserQueryHandler>();
        services.AddScoped<GetUsersQueryHandler>();

        return services;
    }
}
