using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MineHub.Application.Abstractions.Auth;
using MineHub.Application.Abstractions.Email;
using MineHub.Application.Abstractions.Payments;
using MineHub.Application.Abstractions.Persistence;
using MineHub.Application.Abstractions.Users;
using MineHub.Infrastructure.Auth.Entities;
using MineHub.Infrastructure.Auth.Hashing;
using MineHub.Infrastructure.Auth.Jwt;
using MineHub.Infrastructure.Auth.Services;
using MineHub.Infrastructure.Email;
using MineHub.Infrastructure.Payments.Options;
using MineHub.Infrastructure.Payments.Services.YooKassaPayment;
using MineHub.Infrastructure.Persistence;
using MineHub.Infrastructure.Persistence.Repositories;
using MineHub.Infrastructure.Persistence.Seeders;
using System.Text;

namespace MineHub.Infrastructure;

public static class DependencyInjection 
{
    public static IServiceCollection AddInfrastucture(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        
        services.AddScoped<ICurrentIdentityContext, CurrentIdentityContext>();
        services.AddScoped<IDomainUserResolver, DomainUserResolver>();
        services.AddScoped<IAccountService, AccountService>();
        
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

        }).AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

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

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = "localhost:6379";
            options.InstanceName = "MineHub:";
        });

        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IPaymentProvider, YooKassaPaymentProvider>();

        services.Configure<YooKassaOptions>(configuration.GetSection("YooKassa"));
        services.AddHttpClient<YooKassaPaymentProvider>();

        services.AddScoped<IEmailSender, EmailSender>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
