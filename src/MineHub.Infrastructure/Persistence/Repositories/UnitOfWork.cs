using MineHub.Application.Abstractions.Persistence;

namespace MineHub.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken token)
    {
        return _context.SaveChangesAsync(token);
    }
}
