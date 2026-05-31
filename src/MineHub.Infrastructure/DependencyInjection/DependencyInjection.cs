using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MineHub.Application.Abstractions.Persistence;
using MineHub.Application.Abstractions.Services;
using MineHub.Infrastructure.Authentication;
using MineHub.Infrastructure.Identity.Services;
using MineHub.Infrastructure.Persistence.Repositories;
using MineHub.Infrastructure.Persistence.Seeders;

namespace MineHub.Infrastructure.DependencyInjection;

public static class DependencyInjection 
{
    public static IServiceCollection AddInfrastucture(this IServiceCollection services)
    {
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<ICurrentDomainUserService, DomainUserService>();
        services.AddScoped<IIdentityService, IdentityService>();
        
        services.AddScoped<DatabaseSeeder>();
        services.AddScoped<ProductSeeder>();
        services.AddScoped<RoleSeeder>();
        services.AddScoped<IdentitySeeder>();

        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
        services.AddScoped<IRefreshTokenHasher, RefreshTokenHasher>();

        services.AddHttpContextAccessor();

        return services;
    }
}
