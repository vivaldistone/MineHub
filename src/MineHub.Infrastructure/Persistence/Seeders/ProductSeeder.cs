using Microsoft.EntityFrameworkCore;
using MineHub.Domain.Entities;

namespace MineHub.Infrastructure.Persistence.Seeders;

public class ProductSeeder
{
    private readonly AppDbContext _appDbContext;

    public ProductSeeder(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task SeedAsync()
    {
        var premium = new Product("premium", "premium access", 100);
        var sword = new Product("sword", "sword", 250);

        if (!await _appDbContext.Products.AnyAsync(p => p.Name =="premium"))
        {
            await _appDbContext.Products.AddAsync(premium);
        }

        if(!await _appDbContext.Products.AnyAsync(p => p.Name == "sword"))
        {
            await _appDbContext.Products.AddAsync(sword);
        }

        await _appDbContext.SaveChangesAsync();
    }
}
