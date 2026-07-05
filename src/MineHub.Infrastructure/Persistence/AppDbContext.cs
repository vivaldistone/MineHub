using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MineHub.Domain.Entities;
using MineHub.Infrastructure.Auth.Entities;

namespace MineHub.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<AuthUser>
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<User> DomainUsers => Set<User>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Payment> Payments => Set<Payment>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}
