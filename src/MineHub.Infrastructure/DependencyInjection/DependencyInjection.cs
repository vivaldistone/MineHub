using Microsoft.Extensions.DependencyInjection;
using MineHub.Application.Abstractions.Persistence;
using MineHub.Application.Products.Commands.AddProduct;
using MineHub.Application.Products.Commands.DeleteProduct;
using MineHub.Application.Products.Commands.UpdateProduct;
using MineHub.Application.Products.Queries.GetProduct;
using MineHub.Application.Products.Queries.GetProducts;
using MineHub.Infrastructure.Persistence.Repositories;
using MineHub.Infrastructure.Persistence.Seeders;

namespace MineHub.Infrastructure.DependencyInjection;

public static class DependencyInjection 
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<GetProductsQueryHandler>();
        services.AddScoped<GetProductQueryHandler>();
        services.AddScoped<AddProductCommandHandler>();
        services.AddScoped<DeleteProductCommandHandler>();
        services.AddScoped<UpdateProductCommandHandler>();





        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<DatabaseSeeder>();
        services.AddScoped<ProductSeeder>();
        services.AddScoped<RoleSeeder>();
        services.AddScoped<IdentitySeeder>();

        return services;
    }
}
