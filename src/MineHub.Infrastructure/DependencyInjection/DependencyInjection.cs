using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MineHub.Application.Abstractions.Persistence;
using MineHub.Application.Abstractions.Services;
using MineHub.Infrastructure.Authentication;
using MineHub.Infrastructure.Identity;
using MineHub.Infrastructure.Identity.Services;
using MineHub.Infrastructure.Persistence;
using MineHub.Infrastructure.Persistence.Repositories;
using MineHub.Infrastructure.Persistence.Seeders;
using System.Text;

namespace MineHub.Infrastructure.DependencyInjection;

public static class DependencyInjection 
{
    public static IServiceCollection AddInfrastucture(this IServiceCollection services, IConfiguration configuration)
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

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(builderOptions => builderOptions.UseNpgsql(connectionString));

        services.AddIdentityCore<AuthUser>(options =>
        {
            options.Password.RequireNonAlphanumeric = false;

        }).AddRoles<IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var secretKey = configuration["Jwt:SecretKey"];

                if (string.IsNullOrWhiteSpace(secretKey))
                    throw new InvalidOperationException("Jwt sercret key is not configured");

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = "MineHub",
                    ValidAudience = "MineHub-client",

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });


        services.AddHttpContextAccessor();

        return services;
    }
}
