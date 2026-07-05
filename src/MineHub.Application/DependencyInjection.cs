using Microsoft.Extensions.DependencyInjection;
using MineHub.Application.Auth.Commands.Login;
using MineHub.Application.Auth.Commands.RefreshToken;
using MineHub.Application.Auth.Commands.Register;
using MineHub.Application.Auth.Commands.ResetPassword;
using MineHub.Application.Auth.Commands.SendPasswordResetToken;
using MineHub.Application.Carts.Commands.AddItemToCart;
using MineHub.Application.Carts.Commands.ChangeCartItemQuantity;
using MineHub.Application.Carts.Queries.GetCart;
using MineHub.Application.Orders.Commands.CancelOrder;
using MineHub.Application.Orders.Commands.CreateOrderFromCart;
using MineHub.Application.Orders.Queries.GetOrder;
using MineHub.Application.Orders.Queries.GetOrders;
using MineHub.Application.Orders.Queries.GetUserOrder;
using MineHub.Application.Orders.Queries.GetUserOrdersById;
using MineHub.Application.Payments.Commands.CreatePayment;
using MineHub.Application.Payments.Commands.PaymentWebhook;
using MineHub.Application.Products.Commands.AddProduct;
using MineHub.Application.Products.Commands.DeleteProduct;
using MineHub.Application.Products.Commands.UpdateProduct;
using MineHub.Application.Products.Queries.GetProduct;
using MineHub.Application.Products.Queries.GetProducts;
using MineHub.Application.Users.Queries.GetUser;
using MineHub.Application.Users.Queries.GetUsers;

namespace MineHub.Application;

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

        services.AddScoped<LoginUserCommandHandler>();
        services.AddScoped<RegisterUserCommandHandler>();

        services.AddScoped<CancelOrderCommandHandler>();
        services.AddScoped<CreateOrderFromCartCommandHandler>();
        
        services.AddScoped<GetOrderQueryHandler>();
        services.AddScoped<GetOrdersQueryHandler>();
        services.AddScoped<GetUserOrderQueryHandler>();
        services.AddScoped<GetUserOrdersQueryHandler>();
        services.AddScoped<GetUserOrdersByIdQueryHandler>();

        services.AddScoped<RefreshTokenCommandHandler>();

        services.AddScoped<CreatePaymentCommandHandler>();
        services.AddScoped<PaymentWebhookCommandHandler>();

        services.AddScoped<ForgotPasswordCommandHandler>();
        services.AddScoped<ResetPasswordCommandHandler>();

        return services;
    }
}
