using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MineHub.Domain.Entities;
using MineHub.Infrastructure.Identity;

namespace MineHub.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<AuthUser>
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<User> DomainUsers => Set<User>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<Order> Orders => Set<Order>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}
